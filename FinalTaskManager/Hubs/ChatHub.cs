﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FinalTaskManager.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
    }
}