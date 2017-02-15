﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Contracts.Network
{

    public interface IConnectorTcp : IConnectorSocket
    {

        /// <summary>
        /// 连接
        /// </summary>
        void Connect();

    }

}