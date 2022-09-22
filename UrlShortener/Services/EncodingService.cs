using System.Text;

namespace UrlShortener.Services
{
    public class EncodingService : IEncodingService
    {
        private readonly string _characters; 
        private readonly long _base;

        public EncodingService()
        {
            _characters = "0123456789abcdefghijklmnopqrstuvwxyz";
            _base = _characters.Length;
        }

        public string EncodeIntigerToString(long id)
        {
            var builder = new StringBuilder();

            while (id > 0)
            {
                builder.Insert(0, _characters[(int)(id % _base)]);
                id = id / _base;
            }

            return builder.ToString();
        }

        public long DecodeStringToIntiger(string str)
        {
            var id = 0L;
            for (var i = 0; i < str.Length; i++)
            {
                id = id * _base + _characters.IndexOf(str[i]);
            }
            return id;
        }
    }
}
