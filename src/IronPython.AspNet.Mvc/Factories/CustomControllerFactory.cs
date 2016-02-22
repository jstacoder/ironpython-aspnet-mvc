﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace IronPython.AspNet.Mvc
{
    public class CustomControllerFactory : IControllerFactory
    {
        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var rawCtrl = AspNetMvcAPI.Routing.controllers.Where(item => item.Key != null)
                .FirstOrDefault(item => item.Key.ToString().Replace("Controller", "") == controllerName);

            if (rawCtrl.Key == null)
            {
                return null;
            }

            var controller = (IController)MvcApplication.Host.ScriptEngine.Operations.CreateInstance(rawCtrl.Value);

            requestContext.RouteData.Values["controller"] = controllerName;

            return controller;
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(
           System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}