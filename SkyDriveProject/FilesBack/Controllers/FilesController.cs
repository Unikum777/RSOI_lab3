using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FilesBack.Models;
using System.IO;

namespace FilesBack.Controllers
{
    public class FilesController : ApiController
    {
        string logfile = @"C:\Users\Никита\Desktop\filesbacklog.log";
        enum logtype
        {
            Debug,
            Info,
            Warning,
            Error
        }
        private void Log(string loginfo, logtype ltype)
        {
            using (StreamWriter w = System.IO.File.AppendText(logfile))
            {
                switch (ltype)
                {
                    case logtype.Debug:
                        {
                            w.WriteLine(DateTime.Now.ToString("G") + " [FilesBack][DEBUG] " + loginfo);
                            break;
                        }
                    case logtype.Info:
                        {
                            w.WriteLine(DateTime.Now.ToString("G") + " [FilesBack][INFO] " + loginfo);
                            break;
                        }
                    case logtype.Warning:
                        {
                            w.WriteLine(DateTime.Now.ToString("G") + " [FilesBack][WARNING] " + loginfo);
                            break;
                        }
                    case logtype.Error:
                        {
                            w.WriteLine(DateTime.Now.ToString("G") + " [FilesBack][ERROR] " + loginfo);
                            break;
                        }
                    default:
                        {
                            w.WriteLine(DateTime.Now.ToString("G") + " [FilesBack] " + loginfo);
                            break;
                        }
                }
            }
        }
        // GET api/files
        public List<Models.File> Get()
        {
            Log(Request.ToString(), logtype.Debug);
            var files_worker = new FilesWorker();
            var arr1 = Request.Headers.First(p => p.Key == "UserId").Value.ToList<string>();
            string user_id = Convert.ToString(arr1[0]);
            Log("Поиск файлов для пользователя ID = " + user_id, logtype.Info);
            return files_worker.ReadFiles(Convert.ToInt32(user_id));
        }

        // GET api/files/5
        public Models.File Get(int id)
        {
            Log(Request.ToString(), logtype.Debug);
            var files_worker = new FilesWorker();
            var arr1 = Request.Headers.First(p => p.Key == "UserId").Value.ToList<string>();
            string user_id = Convert.ToString(arr1[0]);
            Log("Поиск расширенной информации о файле ID = "+id.ToString() + " для пользователя ID = " + user_id, logtype.Info);
            return files_worker.ReadFile(id, Convert.ToInt32(user_id));
        }

        // POST api/files
        public Models.File Post([FromBody]string value)
        {
            Log(Request.ToString(), logtype.Debug);
            var arr1 = Request.Headers.First(p => p.Key == "file_name").Value.ToList<string>();
            string file_name = Convert.ToString(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "size").Value.ToList<string>();
            int size = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "external_id").Value.ToList<string>();
            string external_id = Convert.ToString(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "user_id").Value.ToList<string>();
            int user_id = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "folder_id").Value.ToList<string>();
            int folder_id = Convert.ToInt32(arr1[0]);

            var files_worker = new FilesWorker();
            Log("Создание файла с именем " + file_name+", пользователь ID = " + user_id, logtype.Info);
            return files_worker.CreateFile(file_name, size, external_id, user_id, folder_id);
        }

        // PUT api/files/5
        public Models.File Put([FromBody]string value)
        {
            Log(Request.ToString(), logtype.Debug);
            var arr1 = Request.Headers.First(p => p.Key == "file_name").Value.ToList<string>();
            string file_name = Convert.ToString(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "size").Value.ToList<string>();
            int size = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "external_id").Value.ToList<string>();
            string external_id = Convert.ToString(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "user_id").Value.ToList<string>();
            int user_id = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "folder_id").Value.ToList<string>();
            int folder_id = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "file_id").Value.ToList<string>();
            int file_id = Convert.ToInt32(arr1[0]);

            var files_worker = new FilesWorker();

            var current_file = files_worker.ReadFile(file_id, user_id);
            current_file.FileName = file_name;
            current_file.Size = size;
            current_file.ExternalId = external_id;
            current_file.FolderId = folder_id;
            Log("Модификация файла с именем " + file_name + ", пользователь ID = " + user_id, logtype.Info);
            return files_worker.UpdateFile(current_file);
        }

        // DELETE api/files/5
        public Models.File Delete()
        {
            Log(Request.ToString(), logtype.Debug);
            var arr1 = Request.Headers.First(p => p.Key == "user_id").Value.ToList<string>();
            int user_id = Convert.ToInt32(arr1[0]);

            arr1 = Request.Headers.First(p => p.Key == "file_id").Value.ToList<string>();
            int file_id = Convert.ToInt32(arr1[0]);

            var files_worker = new FilesWorker();

            var current_file = files_worker.ReadFile(file_id, user_id);
            Log("Удаление файла id = " + file_id.ToString() + ", пользователь ID = " + user_id, logtype.Info);
            return files_worker.DeleteFile(file_id, user_id);
        }
    }
}
