using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AboutJson
{
    public enum Country
    {
        CN,
        EN,
        US
    }
    public class Family 
    {
        public Country Country { set; get; }
        public string Dad { set; get; }
        public string Mum { set; get; }
        public List<string> Children { set; get; }
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class FamilyEx
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Country Country { set; get; }
        [JsonProperty(PropertyName = "Father")]
        public string Dad { set; get; }
        [JsonIgnore]
        public string Mum { set; get; }
        public List<string> Children { set; get; }
    }


    class Program
    {
        static private string TestSerializeObject(Family f)
        {
            return JsonConvert.SerializeObject(f);
        }

        static private Family TestDeserializeObject(string familyJson)
        {
            return JsonConvert.DeserializeObject<Family>(familyJson);
        }

        static private string ConstructJsonManually(Family f)
        {
            JObject familyObj = new JObject();
            JArray childrenObj = new JArray();
            familyObj["Dad"] = f.Dad;
            familyObj[nameof(Family.Mum)] = f.Mum;
            familyObj[nameof(Country)] = f.Country.ToString();
            foreach(var child in f.Children)
            {
                childrenObj.Add(child);
            }
            familyObj[nameof(f.Children)] = childrenObj;
            return familyObj.ToString(Formatting.None);
        }

        static private Family ParseJsonManually(string familyJson)
        {
            Family f = new Family();
            Country country = Country.CN;
            JObject familyObj = JObject.Parse(familyJson);
            if(familyObj[nameof(Country)] != null)
            {
                Enum.TryParse(familyObj[nameof(Country)].ToString(), true, out country);
                f.Country = country;
            }

            if(familyObj[nameof(Family.Dad)] != null)
            {
                f.Dad = familyObj[nameof(Family.Dad)].ToString();
            }

            if(familyObj[nameof(Country)] != null)
            {
                f.Mum = familyObj[nameof(Family.Mum)].ToString();
            }

            if(familyObj[nameof(Family.Children)] != null)
            {
                JArray childrenObj = (JArray)familyObj[nameof(Family.Children)];
                f.Children = new List<string>();
                foreach(var child in childrenObj)
                {
                    f.Children.Add(child.ToString());
                }
            }
            return f;
        }

        static void Main(string[] args)
        {
            Family f = new Family {
                Country = Country.US,
                Dad = "Mr Brown",
                Mum = "Mrs Brown",
                Children = new List<string>{"Lily", "Lucy"} };

            // The result will be
            // { "Country":2,"Dad":"Mr Brown","Mum":"Mrs Brown","Children":["Lily","Lucy"]}
            // But sometimes we need country like this : "Country" : "US"
            string familyJson = TestSerializeObject(f);

            Family f1 = TestDeserializeObject(familyJson);
            
            // The result will be 
            // { "Dad":"Mr Brown","Mum":"Mrs Brown","Country":"US","Children":["Lily","Lucy"]
            string familyJsonManual = ConstructJsonManually(f);

            Family f2 = ParseJsonManually(familyJsonManual);

            // 进阶部分
            FamilyEx fEx = new FamilyEx
            {
                Country = Country.US,
                Dad = "Mr Brown",
                Mum = "Mrs Brown",
                Children = new List<string> { "Lily", "Lucy" }
            };

            // The result will be 
            // { "Country":"US","Father":"Mr Brown","Children":["Lily","Lucy"]}
            string familyJsonEx = JsonConvert.SerializeObject(fEx);
        }
    }
}
