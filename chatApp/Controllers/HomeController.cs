using System;
using System.Collections.Generic;
using System.Diagnostics;
using chatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace chatApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<Tuple<string, string>> chatLogUser1 = new List<Tuple<string, string>>();
        private static List<Tuple<string, string>> chatLogUser2 = new List<Tuple<string, string>>();
        private static string user1Protocol = "long polling";
        private static string user2Protocol = "long polling";
        private static string lastMessageUser1 = "";
        private static string lastMessageUser2 = "";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult User1(string? protocol)
        {
            if(protocol != null)
            {
                user1Protocol = protocol;
            }

            ProtocolDropdownList();
            Chat chat = new Chat(user1Protocol, chatLogUser1);

            //RefreshChat(chat);
            return View(chat);
        }

        public IActionResult User2(string? protocol)
        {
            if (protocol != null)
            {
                user2Protocol = protocol;
            }

            ProtocolDropdownList();
            Chat chat = new Chat(user2Protocol, chatLogUser2);

            //RefreshChat(chat);
            return View(chat);
        }

        public IActionResult Send1(string text)
        {
            if (text == null)
            {
                return RedirectToAction("User1");
            }


            chatLogUser1.Add(new Tuple<string, string>("user 1:", text));
            if (user2Protocol == "webSocket")
            {
                chatLogUser2.Add(new Tuple<string, string>("user 1:", text));
            }

            lastMessageUser1 = text;
            if (user2Protocol == "long polling")
            {
                Get2();
            }

            return RedirectToAction("User1");
        }

        public IActionResult Send2(string text)
        {
            if (text == null)
            {
                return RedirectToAction("User2");
            }

            chatLogUser2.Add(new Tuple<string, string>("user 2:", text));
            if (user1Protocol == "webSocket")
            {
                chatLogUser1.Add(new Tuple<string, string>("user 2:", text));
            }

            lastMessageUser2 = text;

            if (user1Protocol == "long polling")
            {
                Get1();
            }

            return RedirectToAction("User2");
        }
        
        public IActionResult Get1()
        {
            Chat chat;
            if (lastMessageUser2 == "")
            {
                chat = new Chat(user1Protocol, chatLogUser1);
                return RedirectToAction("User1", chat);
            }

            chatLogUser1.Add(new Tuple<string, string>("user 2:", lastMessageUser2));
            chat = new Chat(user1Protocol, chatLogUser1);
            lastMessageUser2 = "";

            return RedirectToAction("User1", chat);
        }

        public IActionResult Get2()
        {
            Chat chat;
            if (lastMessageUser1 == "")
            {
                chat = new Chat(user1Protocol, chatLogUser1);
                return RedirectToAction("User2", chat);
            }

            chatLogUser2.Add(new Tuple<string, string>("user 1:", lastMessageUser1));
            chat = new Chat(user1Protocol, chatLogUser2);
            lastMessageUser1 = "";


            return RedirectToAction("User2", chat);
        }


        /*
        public IActionResult RefreshChat (Chat chat)
        {
            return PartialView("_Chatbox", chat);
        }
        */
        private void ProtocolDropdownList()
        {
            var protocol = new List<string> {"polling", "long polling", "webSocket"};

            ViewBag.Protocol = new SelectList(protocol);
        }

    }
}
