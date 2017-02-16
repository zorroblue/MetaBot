using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Connector;
using System.Diagnostics;
//using HtmlAgilityPack;


namespace MetaBot.Dialogs
{
    [LuisModel("4324327e-a6c0-4275-9318-1d786d57a0b2", "e2763ef1096d49be821a300c978e95d6")]
    [Serializable]

    public class DefaultDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! I am MetaBot. How can I help you?");
            context.Wait(MessageReceived);
        }

        [Serializable]
        public class PartialMessage
        {
            public string Text { set; get; }
        }

        
        [LuisIntent("How to")]
        public async Task HowTo(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Give me a confirmation again. I seem to find an error");
            context.Wait(HowToMesssage);
            
        }

        private async Task HowToMesssage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            Debug.Print("Wrote " + message.Text);
            //await context.PostAsync("You said: " + message.Text);
            var json1 = "";
            string messageText = message.Text;
            messageText = messageText.ToLower();
            System.Text.RegularExpressions.Regex.Replace(messageText, @"\s+", " ");
            messageText = messageText.Replace("?", String.Empty);
            messageText = messageText.Replace(" ", "-");
            
            using (WebClient wc = new WebClient())
            {
                json1 = wc.DownloadString("http://whispering-garden-15641.herokuapp.com/cosine/"+messageText);
                Debug.Print("Finding "+ messageText);
            }

            string x = json1;
            string json2 = json1.Replace("\\", "");
            string json = json2.Replace("\"{", "[{").Replace("}\"", "}]");
            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    var appName = app.Key;
                    var val = app.Value;

                    await context.PostAsync(val.ToString());
                }
            }
        }

        [LuisIntent("get eat spots")]
        public async Task getEatSpots(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Searching places to eat open now !");
            var json1 = "";
            using (WebClient wc = new WebClient())
            {
                json1 = wc.DownloadString("https://whispering-garden-15641.herokuapp.com/polls/");
            }
            int count = 0, count1 = 0;
            string x = json1;
            string json2 = json1.Replace("\\", "");
            string json = json2.Replace("\"[", "[").Replace("]\"", "]");

            int st = 0, ed = 0, flag = 0;
            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                count1 = 0;
                flag = 0;
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    var appName = app.Key;
                    var val = app.Value;

                    if (val.ToString() == "True") flag = 1;
                    if (appName == "start")
                    {
                        if (val.Count() == 0) st = 0;
                        else
                        {
                            string[] start = val[0].ToString().Split('.');
                            string[] start1 = start[0].Split(':');
                            if (val[1].ToString() == "PM") st = Int32.Parse(start1[0]) + 12;
                            else st = Int32.Parse(start1[0].ToString());
                        }
                    }
                    if (appName == "end")
                    {
                        if (val.Count() == 0) ed = -1;
                        else
                        {
                            string[] end = val[0].ToString().Split('.');
                            string[] end1 = end[0].Split(':');

                            if (val[1].ToString() == "PM") ed = Int32.Parse(end1[0]) + 12;
                            else ed = Int32.Parse(end1[0].ToString());
                        }
                    }
                    count1++;
                }

                string hr = DateTime.Now.ToString("HH");
                int now = Int32.Parse(hr);
                if (now >= st && now <= ed && flag > 0)
                {
                    count++;
                    string url = "", cat = "", sta = "", enda = "", name = "";

                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var val = app.Value;

                        if (appName == "name") name = val.ToString();
                        else if (appName == "category") cat = val.ToString();
                        else if (appName == "start") sta = val[0].ToString() + " " + val[1].ToString();
                        else if (appName == "end") enda = val[0].ToString() + " " + val[1].ToString();
                        else if (appName == "url") url = val.ToString();
                    }
                    await context.PostAsync("*******" + count.ToString() + "*****");
                    await context.PostAsync(name);
                    await context.PostAsync("Category : " + cat);
                    await context.PostAsync("start : " + sta + "\t| end : " + enda);
                    await context.PostAsync("url :" + url);
                }

            }
        }

        [LuisIntent("course")]
        public async Task course(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, courseEntered, "Please provide the course code");
        }

        [LuisIntent("news")]
        public async Task news(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync("Which sources to search ?");
            List<string> BotOptions = new List<string>();
            BotOptions.Add("Awaaz");
            BotOptions.Add("Gymkhana");
            BotOptions.Add("IIT Kgp Tech");
            BotOptions.Add("Scholar's Avenue");
            PromptDialog.Choice(context,
                   AfterUserHasChosenAsync, BotOptions,
                   "Select the source !",
                   "Didn't get that",
                   1,
                   PromptStyle.Auto);


        }

        private async Task courseEntered(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;
            context.Wait(MessageReceived);
            if (choice.Length != 7 || choice[0] < 65 || choice[0] > 90 || choice[1] < 65 || choice[1] > 90)
            {
                await context.PostAsync("Wrong course code format ! Try Again !");
            }
            else
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://raw.githubusercontent.com/zorroblue/blackjack/master/allCourses.json");
                }

                int count = 0, count1 = 0;
                var objects = JArray.Parse(json); // parse as array  
                foreach (JObject root in objects)
                {
                    count++;
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var EX = (String)app.Value["EX"];
                        var A = (String)app.Value["A"];
                        var B = (String)app.Value["B"];
                        var C = (String)app.Value["C"];
                        var D = (String)app.Value["D"];
                        var P = (String)app.Value["P"];
                        var F = (String)app.Value["F"];
                        var X = (String)app.Value["X"];

                        if (appName == choice)
                        {
                            count1++;
                            await context.PostAsync("EX : " + EX.ToString());
                            await context.PostAsync("A : " + A.ToString());
                            await context.PostAsync("B : " + B.ToString());
                            await context.PostAsync("C : " + C.ToString());
                            await context.PostAsync("D : " + D.ToString());
                            await context.PostAsync("P : " + P.ToString());
                            await context.PostAsync("F : " + F.ToString());
                            await context.PostAsync("X : " + X.ToString());
                        }
                    }
                }
                if (count1 == 0) await context.PostAsync("Course not found :(");
            }
        }


        private async Task AfterUserHasChosenAsync(IDialogContext context, IAwaitable<string> result)
        {
            string userChoice = await result;
            context.Wait(MessageReceived);

            if (userChoice == "Awaaz")
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://raw.githubusercontent.com/metakgp/naarad/master/docs/awaaziitkgp.json");
                }
                int count = 0, count1 = 0;
                var objects = JArray.Parse(json); // parse as array  
                foreach (JObject root in objects)
                {
                    count++;
                    count1 = 0;
                    if (count == 6) break;
                    await context.PostAsync(count.ToString());
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var val = app.Value;

                        if (appName != "pic" && appName != "id") await context.PostAsync(val.ToString());

                        count1++;
                    }
                }
            }


            else if (userChoice == "Gymkhana")
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://raw.githubusercontent.com/metakgp/naarad/master/docs/TSG.IITKharagpur.json");
                }
                int count = 0, count1 = 0;
                var objects = JArray.Parse(json); // parse as array  
                foreach (JObject root in objects)
                {
                    count++;
                    count1 = 0;
                    if (count == 6) break;
                    await context.PostAsync(count.ToString());
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var val = app.Value;

                        if (appName != "pic" && appName != "id") await context.PostAsync(val.ToString());

                        count1++;
                    }
                }
            }


            else if (userChoice == "IIT Kgp Tech")
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://raw.githubusercontent.com/metakgp/naarad/master/docs/iitkgp.tech.json");
                }
                int count = 0, count1 = 0;
                var objects = JArray.Parse(json); // parse as array  
                foreach (JObject root in objects)
                {
                    count++;
                    count1 = 0;
                    if (count == 6) break;
                    await context.PostAsync(count.ToString());
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var val = app.Value;

                        if (appName != "pic" && appName != "id") await context.PostAsync(val.ToString());

                        count1++;
                    }
                }
            }


            else if (userChoice == "Scholar's Avenue")
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://raw.githubusercontent.com/metakgp/naarad/master/docs/scholarsavenue.json");
                }
                int count = 0, count1 = 0;
                var objects = JArray.Parse(json); // parse as array  
                foreach (JObject root in objects)
                {
                    count++;
                    count1 = 0;
                    if (count == 6) break;
                    await context.PostAsync(count.ToString());
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        var appName = app.Key;
                        var val = app.Value;

                        if (appName != "pic" && appName != "id") await context.PostAsync(val.ToString());

                        count1++;
                    }
                }
            }
        }
    }
}