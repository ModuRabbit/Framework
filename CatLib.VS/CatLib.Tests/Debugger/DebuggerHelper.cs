﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using CatLib.API.Config;
using CatLib.Config;
using CatLib.Core;
using CatLib.Debugger;
using CatLib.Json;
using CatLib.Routing;

namespace CatLib.Tests.Debugger
{
    public static class DebuggerHelper
    {
        public static Application GetApplication()
        {
            var app = new Application();
            app.OnFindType((str) => Type.GetType(str));
            app.Bootstrap();
            app.Register(new RoutingProvider());
            app.Register(new JsonProvider());
            app.Register(new DebuggerProvider());
            app.Register(new ConfigProvider());
            app.Make<IConfigManager>().Default.Set("debugger.logger.handler.unity", false);
            app.Init();
            return app;
        }
    }
}
