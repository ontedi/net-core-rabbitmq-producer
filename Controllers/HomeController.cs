using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<kisiler> kisilistesi = new List<kisiler>();
            kisilistesi.Add(new kisiler { isim = "Serkan", mail = "serkan@google.com", mesaj = "Merhaba Serkan. Ödeme mevcut" });
            kisilistesi.Add(new kisiler { isim = "Ali", mail = "ali@yahoo.com", mesaj = "Merhaba Ali. Hoşgeldin" });
            kisilistesi.Add(new kisiler { isim = "Ahmed", mail = "ahmed@google.com", mesaj = "Merhaba AHmed. İyi ki doğdun" });

            ConnectionFactory cfBaglantiBilgileri = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using (var cfBaglanti = cfBaglantiBilgileri.CreateConnection())
            using (var chnKanal = cfBaglanti.CreateModel())
            {
                chnKanal.QueueDeclare
                (
                    queue: "bilgilendirme-mesajlari-2022-01-15",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                string strJson = JsonConvert.SerializeObject(kisilistesi);
                byte[] bytMesajIcerigi = Encoding.UTF8.GetBytes(strJson);

                chnKanal.BasicPublish
                (
                    exchange: "",
                    routingKey: "bilgilendirme-mesajlari-2022-01-15",
                    basicProperties: null,
                    body: bytMesajIcerigi
                );
            }

            return View();
        }
        public class kisiler
        {
            public string isim { get; set; }
            public string mail { get; set; }
            public string mesaj { get; set; }
        }

    }
}
