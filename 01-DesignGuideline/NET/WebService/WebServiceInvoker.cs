/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：WebServiceInvoker.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时02分00秒
 * * 文件标识：85A3FD5B-02D7-420A-98CE-89D7FBF10430
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Net;
using System.Web.Services.Description;
using System.IO;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;

namespace Codest.Net.WebService
{
    /// <summary>
    /// 实现动态调用Web Service
    /// </summary>
    public class WebServiceInvoker
    {
        #region public object InvokeWebService(string url, string methodname, object[] args)
        /// <summary>
        /// 动态调用Web Service
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="methodName">要调用Web Service的方法名称</param>
        /// <param name="args">要调用Web Service的参数列表</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string methodName, object[] args)
        {
            return this.InvokeWebService(url, null, methodName, args);
        }
        #endregion

        #region public object InvokeWebService(string url, string methodname, object arg)
        /// <summary>
        /// 动态调用Web Service，传入单个参数
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="methodName">调用Web Service的方法名称</param>
        /// <param name="arg">要调用Web Service的参数</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string methodName, object arg)
        {
            return this.InvokeWebService(url, null, methodName, new object[] { arg });
        }
        #endregion

        #region public object InvokeWebService(string url, string classname, string methodname, object[] args)
        /// <summary>
        /// 动态调用Web Service
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="className">要调用Web Service的类的名称</param>
        /// <param name="methodName">要调用Web Service的方法名称</param>
        /// <param name="args">要调用Web Service的参数列表</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string className, string methodName, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((className == null) || (className == ""))
            {
                className = this.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(url + "?WSDL");
                ServiceDescription serviceDescription = ServiceDescription.Read(stream);
                ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter();
                serviceDescriptionImporter.AddServiceDescription(serviceDescription, "", "");
                CodeNamespace codeNamespace = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(codeNamespace);
                serviceDescriptionImporter.Import(codeNamespace, codeCompileUnit);
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                ICodeCompiler codeCompiler = codeProvider.CreateCompiler();

                //设定编译参数
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults compilerResults = codeCompiler.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
                if (true == compilerResults.Errors.HasErrors)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        stringBuilder.Append(compilerError.ToString());
                        stringBuilder.Append(System.Environment.NewLine);
                    }
                    throw new Exception(stringBuilder.ToString());
                }

                //生成代理实例，并调用方法
                Assembly assembly = compilerResults.CompiledAssembly;
                Type type = assembly.GetType(@namespace + "." + className, true, true);
                object obj = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);

                return methodInfo.Invoke(obj, args);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message, new Exception(exception.InnerException.StackTrace));
            }
        }
        #endregion

        #region private string GetWsClassName(string wsUrl)
        /// <summary>
        /// 通过Web Service的URL获取类名
        /// </summary>
        /// <param name="webServiceURL">Web Service的URL地址</param>
        /// <returns>Web Service的类名</returns>
        private string GetWsClassName(string webServiceURL)
        {
            string[] parts = webServiceURL.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }

}
