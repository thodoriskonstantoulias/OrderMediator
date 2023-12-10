using OrderMediator.Models;
using System.Xml.Serialization;
using System.Xml;

namespace OrderMediator.Extensions
{
    public static class OrderExtensions
    {
        public static string TransformToXml(this OrderModel orderModel)
        {
            XmlSerializer ser = new XmlSerializer(typeof(OrderModel));

            using (var sww = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented })
                {
                    ser.Serialize(writer, orderModel);
                    return sww.ToString();
                }
            }
        }
    }
}
