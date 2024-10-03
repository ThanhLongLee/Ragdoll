using Unity.Common.Configuration;
using Unity.Common.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Unity.Common.Helper
{
    public class FileHelper
    {
        public static string ProductImageName(string objectId, string type = "")
        {
            var websiteName = "gomi";
            type = string.IsNullOrEmpty(type) ? "" : type + "_";
            return websiteName + "_" + type + objectId + "-" + DateExtensions.UtcNowTicks;
        }

        /// <summary>
        /// Lấy đường dẫn vật lý folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static string ResourcesPath(string folderPath)
        {
            folderPath = folderPath.Replace("/", "\\");

            string parent = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~"));
            string grandParent = Path.GetDirectoryName(parent);

            return Path.Combine(grandParent + "\\" + AppSettings.Resources + "\\" + folderPath);
        }

        public static string SaveImageFile(string commands, string fileDir, string imageName, int resize)
        {
            Debug.WriteLine(fileDir);
            Debug.WriteLine(Directory.Exists(fileDir));

            //check if the folder exists;
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            string newFile = imageName + ".jpg";
            string filePath = Path.Combine(fileDir, newFile);

            byte[] bytes = Convert.FromBase64String(commands);

            if (bytes.Length > 0)
            {
                MemoryStream stream = new MemoryStream(bytes, 0, bytes.Length);
                using (var image = Image.FromStream(stream))
                {
                    // Resize Image
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    float ratioX = resize / (float)originalWidth;
                    float ratioY = resize / (float)originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    var resizingImg = new Bitmap(newWidth, newHeight);
                    var resizeGraph = Graphics.FromImage(resizingImg);
                    resizeGraph.CompositingQuality = CompositingQuality.HighQuality;
                    resizeGraph.SmoothingMode = SmoothingMode.HighQuality;
                    resizeGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    resizeGraph.DrawImage(image, imageRectangle);

                    resizingImg.Save(filePath, image.RawFormat);
                    resizingImg.Dispose();

                    return newFile;
                }
            }

            return "";
        }

        public static bool RemoveFile(string fileDir, string imageName)
        {
            try
            {
                string fileName = imageName + ".jpg";
                string path = Path.Combine(fileDir, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Di chuyển file hình sang thư mục khác.
        ///
        /// </summary>
        /// <param name="objectId">Id của object</param>
        /// <param name="file">tên file hiện tại</param>
        /// <param name="currentFolder">thư mục hiện tại của file</param>
        /// <param name="folderTarget">Thư sẽ di chuyển tới</param>
        /// <param name="newFileName">Tên file(nếu có). Nếu không truyền tên file sẽ lấy tên ngẫu nhiên.</param>
        /// <returns></returns>
        public static string MoveFile(string objectId, string file, string currentFolder, string folderTarget, string newFileName = null)
        {
            var newFile = string.Empty;
            try
            {
                var splitImg = file.Split('.');
                var currentImgName = splitImg.First() + "." + splitImg.Last();

                // get file in folder Temp
                var sourceFile = currentFolder + "\\" + currentImgName;

                // get new file name
                if (string.IsNullOrEmpty(newFileName))
                {
                    newFile = FileHelper.ProductImageName(objectId) + "." + splitImg.Last();
                }
                else
                {
                    newFile = newFileName + "." + splitImg.Last();
                }
                // get full path by folder target
                var fullPath = FileHelper.ResourcesPath(folderTarget);

                // target folder save file
                var destinationFile = fullPath + newFile;

                // If it doesn't exists then create folder
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                // If file Exists then Delete
                if (System.IO.File.Exists(destinationFile))
                    System.IO.File.Delete(destinationFile);

                // To move a file or folder to a new location:
                if (System.IO.File.Exists(sourceFile))
                    System.IO.File.Move(sourceFile, destinationFile);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
            return newFile;
        }

        /// <summary>
        /// Lưu hình ảnh dành cho web aplication.
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="newImgName">Tên file sẽ lưu</param>
        /// <param name="root">Thư mục sẽ lưu</param>
        /// <param name="rootMarker">Marker hình ảnh</param>
        /// <param name="resize"></param>
        /// <returns></returns>
        public static string SaveImageByWeb(HttpPostedFileBase fileData, string newImgName, string root, string rootMarker, int resize)
        {
            string newFile = string.Empty;
            string fileName = fileData.FileName;
            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                fileName = fileName.Trim('"');

            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                fileName = Path.GetFileName(fileName);
            string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1, 3);
            newFile = newImgName + "." + fileExt;
            string filePath = Path.Combine(root, newFile);
            Stream strm = fileData.InputStream;
            if (resize != 0)
            {
                using (var image = Image.FromStream(strm))
                {
                    // Resize Image
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    float ratioX = resize / (float)originalWidth;
                    float ratioY = resize / (float)originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    var resizingImg = new Bitmap(newWidth, newHeight);
                    var resizeGraph = Graphics.FromImage(resizingImg);
                    resizeGraph.CompositingQuality = CompositingQuality.HighQuality;
                    resizeGraph.SmoothingMode = SmoothingMode.HighQuality;
                    resizeGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    resizeGraph.DrawImage(image, imageRectangle);

                    if (!string.IsNullOrEmpty(rootMarker))
                    {
                        // Add Marker
                        using (Image watermarkImage = Image.FromFile(rootMarker))
                        using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                        {
                            int x = (resizingImg.Width - 80) - watermarkImage.Width;
                            int y = (resizingImg.Height - 60) - watermarkImage.Height;
                            watermarkBrush.TranslateTransform(x, y);
                            resizeGraph.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width, watermarkImage.Height)));

                        }
                    }
                    resizingImg.Save(filePath, image.RawFormat);
                    resizingImg.Dispose();
                }
            }
            else { fileData.SaveAs(filePath); }

            return newFile;
        }

        // thay doi huong, marker image
        public static bool UpdateImageFile(string imgPath, string rootMarker, int rotate)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(imgPath);
                var ms = new MemoryStream(bytes);

                using (var image = Image.FromStream(ms))
                {
                    if (rotate > 0)
                    {
                        RotateFlipType rotateFlip;
                        switch (rotate)
                        {
                            case 90:
                                rotateFlip = RotateFlipType.Rotate90FlipNone;
                                break;
                            case 180:
                                rotateFlip = RotateFlipType.Rotate180FlipNone;
                                break;
                            case 270:
                                rotateFlip = RotateFlipType.Rotate270FlipNone;
                                break;
                            default:
                                rotateFlip = RotateFlipType.RotateNoneFlipNone;
                                break;
                        }
                        image.RotateFlip(rotateFlip);
                    }


                    var resizeGraph = Graphics.FromImage(image);
                    resizeGraph.CompositingQuality = CompositingQuality.HighQuality;
                    resizeGraph.SmoothingMode = SmoothingMode.HighQuality;
                    resizeGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    if (!string.IsNullOrEmpty(rootMarker))
                    {
                        // Add Marker
                        using (Image watermarkImage = Image.FromFile(rootMarker))
                        using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                        {
                            int x = (image.Width - 80) - watermarkImage.Width;
                            int y = (image.Height - 60) - watermarkImage.Height;
                            watermarkBrush.TranslateTransform(x, y);
                            resizeGraph.FillRectangle(watermarkBrush,
                                new Rectangle(new Point(x, y), new Size(watermarkImage.Width, watermarkImage.Height)));

                        }
                    }
                    image.Save(imgPath, image.RawFormat);
                    image.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }

    }


    public static class HttpPostedFileBaseExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }

            return true;
        }
    }
}
