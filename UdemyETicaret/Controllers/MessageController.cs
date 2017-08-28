using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UdemyETicaret.Filter;
using UdemyETicaret.Models.Message;

namespace UdemyETicaret.Controllers
{
    [MyAuthorization]
    public class MessageController : BaseController
    {
        // GET: Message
        [HttpGet]
        public ActionResult i()
        {
            if (IsLogon() == false) return RedirectToAction("index", "i");
            var currentId = CurrentUserId();
            Models.Message.iModels model = new Models.Message.iModels();
            #region - Select List Item -
            model.Users = new List<SelectListItem>();
            var users = context.Members.Where(x => ((int)x.MemberType) > 0 && x.Id != currentId).ToList();
            model.Users = users.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = string.Format("{0} {1} ({2})", x.Name, x.Surname, x.MemberType.ToString())
            }).ToList();
            #endregion
            #region - Mesaj Listesi -
            var mList = context.Messages.Where(x => x.ToMemberId == currentId || x.MessageReplies.Any(y => y.Member_Id == currentId)).ToList();
            model.Messages = mList;
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult SendMessage(Models.Message.SendMessageModel message)
        {
            if (IsLogon() == false) return RedirectToAction("index", "i");

            DB.Messages mesaj = new DB.Messages()
            {
                Id = Guid.NewGuid(),
                AddedDate = DateTime.Now,
                IsRead = false,
                Subject = message.Subject,
                ToMemberId = message.ToUserId
            };
            var mRep = new DB.MessageReplies()
            {
                Id = Guid.NewGuid(),
                AddedDate = DateTime.Now,
                Member_Id = CurrentUserId(),
                Text = message.MessageBody
            };
            mesaj.MessageReplies.Add(mRep);
            context.Messages.Add(mesaj);
            context.SaveChanges();
            return RedirectToAction("i", "Message");
        }
        [HttpGet]
        public ActionResult MessageReplies(string id)
        {
            if (IsLogon() == false) return RedirectToAction("index", "i");
            var currentId = CurrentUserId();
            var guid = new Guid(id);
            DB.Messages message = context.Messages.FirstOrDefault(x => x.Id == guid);
            if (message.ToMemberId== currentId)
            {
                message.IsRead = true;
                context.SaveChanges();
            }

            MessageRepliesModel model = new MessageRepliesModel();
            model.MReplies = context.MessageReplies.Where(x => x.MessageId == guid).OrderBy(x => x.AddedDate).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult MessageReplies(DB.MessageReplies message)
        {
            if (IsLogon() == false) return RedirectToAction("index", "i");

            message.AddedDate = DateTime.Now;
            message.Id = Guid.NewGuid();
            message.Member_Id = CurrentUserId();
            context.MessageReplies.Add(message);
            context.SaveChanges();
            return RedirectToAction("MessageReplies", "Message", new { id = message.MessageId });
        }

        [HttpGet]
        public ActionResult RenderMessage()
        {
            RenderMessageModel model = new RenderMessageModel();
            var currentId = CurrentUserId();
            var mList = context.Messages
                        .Where(x => x.ToMemberId == currentId || x.MessageReplies.Any(y => y.Member_Id == currentId))
                        .OrderByDescending(x => x.AddedDate);
            model.Messages = mList.Take(4).ToList();
            model.Count = mList.Count();

            return PartialView("_Message", model);
        }

        public ActionResult RemoveMessageReplies(string id)
        {
            var guid = new Guid(id);
            //mesja cepaları silindi
            var mReplies = context.MessageReplies.Where(x => x.MessageId == guid);
            context.MessageReplies.RemoveRange(mReplies);
            //mesajın kendisi silindi.
            var message = context.Messages.FirstOrDefault(x => x.Id == guid);
            context.Messages.Remove(message);

            context.SaveChanges();

            return RedirectToAction("i", "Message");
        }
    }
}