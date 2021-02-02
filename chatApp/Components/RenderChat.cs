using chatApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatApp.Components
{
    public class RenderChat : ViewComponent
    {
        public RenderChat() { }
        public IViewComponentResult Invoke(Chat chat)
        {
            return View("_Chatbox", chat);
        }
    }
}
