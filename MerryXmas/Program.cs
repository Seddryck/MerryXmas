using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MerryXmas
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Player> senders = Initialize(); ;
            IList<Player> recipients = null;
            var list = new List<Gift>();
            var random = new Random();
            var count = 0;
            do
            {
                count++;
                Console.WriteLine("Try: {0}", count);

                recipients = Initialize();
                list.Clear();

                for (int i = 0; i < senders.Count(); i++)
                {
                    var index = random.Next(recipients.Count());
                    if (senders.ElementAt(i).Name != recipients.ElementAt(index).Name)
                    //We can't send to ourselves
                    {
                        list.Add(new Gift() { Sender = senders.ElementAt(i), Recipient = recipients.ElementAt(index) });
                        recipients.RemoveAt(index);
                    }
                }
            }
            while (list.Count() != senders.Count());

            SendEmails(list);

        }

        private static void SendEmails(List<Gift> list)
        {
            foreach (var gift in list)
            {
                
                var client = new SmtpClient()
                {
                    Port = 587,
                    Host = "smtp.googlemail.com",
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "yourpassword"),
                };
                var mail = new MailMessage("seddryck@gmail.com", gift.Sender.Email);
                mail.Bcc.Add("bcc@gmail.be");
                mail.Subject = "Cadeau de Noël";
                
                var text = "Bonjour {0},\r\n\r\n"
                    + "la personne à qui tu vas offrir ton cadeau de Noël est {1}.\r\n\r\n"
                    + "Bon shopping, \r\n\r\n"
                    + "Le père Noël ;o)";
                mail.Body = string.Format(text, gift.Sender.Name.Split(new [] {' '})[0], gift.Recipient.Name);
                try
                {
                    Console.WriteLine("Sending e-mail to {0}", mail.To);
                    client.Send(mail);
                }
                catch (Exception ex )
                {
                    
                    throw ex ;
                }
            }
        }

        private static IList<Player> Initialize()
        {
            var players = new List<Player>();
            players.Add(new Player() { Name = "1", Email = "xxx@gmail.com" });
            players.Add(new Player() { Name = "2", Email = "xx2@gmail.com" });
            players.Add(new Player() { Name = "3", Email = "xx3@gmail.com" });
            players.Add(new Player() { Name = "4", Email = "xx4@gmail.com" });
            players.Add(new Player() { Name = "5", Email = "xx5@gmail.com" });
            players.Add(new Player() { Name = "6", Email = "xx6@gmail.com" });
            return players;
        }
    }
}
