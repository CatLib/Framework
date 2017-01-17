using UnityEngine;
using System.Collections;
using CatLib.Base;

namespace CatLib.UpdateSystem
{
    public class CAutoUpdateProvider : CServiceProvider
    {

        public CAutoUpdateProvider(CApplication app) : base(app)
        {
            
        }

        public override void Init()
        {
            base.Init();

            Debug.Log(application);
            Debug.Log("CAutoUpdateProvider 初始化了");
        }

        public override void Register()
        {
            Debug.Log("CAutoUpdateProvider 注册了一个内容");
        }

    }

}