using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// <para>Utility functions to cast Newtonsoft json objects into common types such as Dictionary and List of Dictionary</para>
/// </summary>
public class JsonUtil
{
    /// <summary>
    /// json is expected to be a standard map
    /// </summary>
    public static Dictionary<string,dynamic> jsonToDynamic(string json)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.None;
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json, settings);
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to Deserialize json into Dnyamic. Given json was = ${json}");
        }
    }
    
    /// <summary>
    /// json is expected to be a standard map
    /// </summary>
    public static Dictionary<string,dynamic> jsonToMap(string json)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.None;
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json, settings);
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to Deserialize json into Dnyamic. Given json was = ${json}");
        }
    }

    /// <summary>
    /// <para>jObject is expected to be a Dictionary.</para>
    /// <para>Common use case is when the mainMap is a Dictionary and the value (presented as object) is also a Dictionary. </para>
    /// <para>Use this function to cast that value into another Dictionary </para>
    /// </summary>
    public static Dictionary<string,object> jObjectToMap(object jObject)
    {
        try
        {
            return (jObject as JObject).ToObject<Dictionary<string, object>>();
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to convert jObject to Map. Given jObject is type={jObject.GetType()}");
        }
    }


    /// <summary>
    /// <para>jArray is expected to be a List of Dictionary</para>
    /// <para>Common use case is when the mainMap is a Dictionary and the value (presented as object) is type List of Dictionary.</para>
    /// <para>Use this function to cast that value into a proper List of Dictionary </para>
    /// </summary>
    public static List<Dictionary<string, object>> jArrayToListMap(object jArray)
    {
        try
        {
            return (jArray as JArray).ToObject<List<Dictionary<string, object>>>();
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to convert jArray to List<Dictionary>. Given jArray is type={jArray.GetType()}");
        }
    }

    public static List<object> jArrayToListObject(object jArray)
    {
        try
        {
            return (jArray as JArray).ToObject<List<object>>();
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to convert jArray to List<Object>. Given jArray is type={jArray.GetType()}");
        }
    }

    public static List<Dictionary<string, object>> JsonToListMap(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<List<Dictionary<string,object>>>(json);
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to Deserialize json into List of Dictionary. Given json was = ${json}");
        }
    }

    public static string toJson(object _object, bool isPrettyPrint = false)
    {
        try
        {
            return JsonConvert.SerializeObject(_object, isPrettyPrint ? Formatting.Indented : Formatting.None);
        }
        catch (Exception)
        {
            throw new Exception($"Exception at MyJsonUtil failed to convert _object into json. _object is type={_object.GetType()}");
        }
    }

}

