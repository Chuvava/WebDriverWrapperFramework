using Newtonsoft.Json.Linq;

namespace WebDriverWrapper
{
    public class SeleniumWrapper
    {
        public void ParseJson(string json)
        {
            //{"FirstName":"Roei","LastName":"Sabag","Age":34,"LikeThisCourse":true}
            var ourJsonObject = JObject.Parse(json);

            if ((bool)ourJsonObject["LikeThisCourse"])
            {
                var firstName = (string)ourJsonObject["FirstName"];
                var lastName = (string)ourJsonObject["LastName"];
                var age = (int)ourJsonObject["Age"];
            }
        }
    }
}
