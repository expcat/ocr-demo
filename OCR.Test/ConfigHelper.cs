using Microsoft.Extensions.Configuration;

namespace OCR.Test
{
    public class ConfigHelper
    {
        public readonly static string Appid;
        public readonly static string SecretId;
        public readonly static string SecretKey;
        public readonly static string Bucket;
        public readonly static string Fileid;

        public readonly static string BDAppid;
        public readonly static string BDApiKey;
        public readonly static string BDSecretKey;

        static ConfigHelper()
        {
            var conf = new ConfigurationBuilder().AddJsonFile("secret.json").Build();
            Appid = conf["qcloud:appid"];
            SecretId = conf["qcloud:secretId"];
            SecretKey = conf["qcloud:secretKey"];
            Bucket = conf["qcloud:bucket"];
            Fileid = conf["qcloud:fileid"];
            BDAppid = conf["baidu:appid"];
            BDApiKey = conf["baidu:apiKey"];
            BDSecretKey = conf["baidu:secretKey"];
        }

    }
}
