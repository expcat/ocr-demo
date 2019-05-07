using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OCR.Test.Models;
using RestSharp;
using Baidu.Aip.Ocr;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace OCR.Test.Controllers
{
    public class OCRController : Controller
    {
        #region QCloud

        [HttpGet]
        public IActionResult TXIDCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TXIDCard(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var frontClient = new RestClient("http://recognition.image.myqcloud.com/ocr/idcard");
                var frontRequest = new RestRequest(Method.POST);
                frontRequest.AddHeader("Host", "recognition.image.myqcloud.com");
                frontRequest.AddHeader("Content-Type", "multipart/form-data");
                var sign = OCRHelper.HmacSha1Sign();
                frontRequest.AddHeader("Authorization", sign);
                frontRequest.AlwaysMultipartFormData = true;
                frontRequest.AddParameter("appid", ConfigHelper.Appid);
                frontRequest.AddParameter("card_type", "0");

                using (var stream = model.FrontFile.OpenReadStream())
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    frontRequest.AddFileBytes("image[0]", bytes, "0.jpg", "image/jpeg");
                }
                var frontResponse = frontClient.Execute(frontRequest);
                var frontStr = frontResponse.Content;
                var backStr = string.Empty;

                if (model.BackFile != null)
                {
                    var backClient = new RestClient("http://recognition.image.myqcloud.com/ocr/idcard");
                    var backRequest = new RestRequest(Method.POST);
                    backRequest.AddHeader("Host", "recognition.image.myqcloud.com");
                    backRequest.AddHeader("Content-Type", "multipart/form-data");
                    backRequest.AddHeader("Authorization", OCRHelper.HmacSha1Sign());
                    backRequest.AlwaysMultipartFormData = true;
                    backRequest.AddParameter("appid", ConfigHelper.Appid);
                    backRequest.AddParameter("card_type", "1");

                    using (var stream = model.BackFile.OpenReadStream())
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);
                        backRequest.AddFileBytes("image[0]", bytes, "0.jpg", "image/jpeg");
                    }

                    var backResponse = backClient.Execute(backRequest);
                    backStr = backResponse.Content;
                }

                if (frontResponse.IsSuccessful)
                {
                    var frontData = JsonConvert.DeserializeObject<IDCardViewModel>(frontStr);
                    if (model.BackFile != null)
                    {
                        var backData = JsonConvert.DeserializeObject<IDCardViewModel>(backStr);
                        frontData.result_list.AddRange(backData.result_list);
                    }
                    ViewData["result"] = frontData;
                }
                else
                {
                    ViewData["error"] = frontStr;
                }

            }

            return View();
        }

        [HttpGet]
        public IActionResult TXDrivingLicence()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TXDrivingLicence(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var frontClient = new RestClient("http://recognition.image.myqcloud.com/ocr/drivinglicence");
                var frontRequest = new RestRequest(Method.POST);
                frontRequest.AddHeader("Host", "recognition.image.myqcloud.com");
                frontRequest.AddHeader("Content-Type", "multipart/form-data");
                var sign = OCRHelper.HmacSha1Sign();
                frontRequest.AddHeader("Authorization", sign);
                frontRequest.AlwaysMultipartFormData = true;
                frontRequest.AddParameter("appid", ConfigHelper.Appid);
                frontRequest.AddParameter("type", "1");

                using (var stream = model.FrontFile.OpenReadStream())
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    frontRequest.AddFileBytes("image", bytes, "0.jpg", "image/jpeg");
                }
                var frontResponse = frontClient.Execute(frontRequest);
                var frontStr = frontResponse.Content;

                if (frontResponse.IsSuccessful)
                {
                    var frontData = JsonConvert.DeserializeObject<DrivingLicenceViewModel>(frontStr);
                    ViewData["result"] = frontData;
                }
                else
                {
                    ViewData["error"] = frontStr;
                }

            }

            return View();
        }

        #endregion

        #region Baidu

        [HttpGet]
        public IActionResult BDIDCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BDIDCard(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new Dictionary<string, object>{
                    {"detect_direction", "false"},
                    {"detect_risk", "true"}
                };

                var client = new Ocr(ConfigHelper.BDApiKey, ConfigHelper.BDSecretKey);
                client.Timeout = 60000;  // 修改超时时间
                Dictionary<string, string> idCardInfo = new Dictionary<string, string>();
                using (var stream = model.FrontFile.OpenReadStream())
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    // 带参数调用身份证识别
                    var result = client.Idcard(bytes, "front", options);
                    var words_result = result["words_result"];
                    if (words_result != null && words_result.HasValues)
                    {
                        foreach (var item in words_result.Children<JProperty>())
                        {
                            var words = item.Children()["words"];
                            idCardInfo.Add(item.Name, words.Values<JValue>().First().Value.ToString());
                        }
                    }
                }
                if (model.BackFile != null)
                {
                    using (var stream = model.BackFile.OpenReadStream())
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);
                        // 带参数调用身份证识别
                        var result = client.Idcard(bytes, "back", options);
                        var words_result = result["words_result"];
                        if (words_result != null && words_result.HasValues)
                        {
                            foreach (var item in words_result.Children<JProperty>())
                            {
                                var words = item.Children()["words"];
                                idCardInfo.Add(item.Name, words.Values<JValue>().First().Value.ToString());
                            }
                        }
                    }
                }
                ViewData["result"] = idCardInfo;
            }
            return View();
        }

        [HttpGet]
        public IActionResult BDDrivingLicence()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BDDrivingLicence(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new Dictionary<string, object>{
                    {"detect_direction", "false"}
                };

                var client = new Ocr(ConfigHelper.BDApiKey, ConfigHelper.BDSecretKey);
                client.Timeout = 60000;  // 修改超时时间
                Dictionary<string, string> idCardInfo = new Dictionary<string, string>();
                using (var stream = model.FrontFile.OpenReadStream())
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    // 带参数调用身份证识别
                    var result = client.DrivingLicense(bytes, options);
                    var words_result = result["words_result"];
                    if (words_result != null && words_result.HasValues)
                    {
                        foreach (var item in words_result.Children<JProperty>())
                        {
                            var words = item.Children()["words"];
                            idCardInfo.Add(item.Name, words.Values<JValue>().First().Value.ToString());
                        }
                    }
                }
                ViewData["result"] = idCardInfo;
            }
            return View();
        }

        #endregion
    }
}
