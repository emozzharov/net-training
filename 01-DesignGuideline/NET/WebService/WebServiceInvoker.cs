/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�WebServiceInvoker.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��00��
 * * �ļ���ʶ��85A3FD5B-02D7-420A-98CE-89D7FBF10430
 * * ����ժҪ��
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
    /// ʵ�ֶ�̬����Web Service
    /// </summary>
    public class WebServiceInvoker
    {
        #region public object InvokeWebService(string url, string methodname, object[] args)
        /// <summary>
        /// ��̬����Web Service
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="methodName">Ҫ����Web Service�ķ�������</param>
        /// <param name="args">Ҫ����Web Service�Ĳ����б�</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string methodName, object[] args)
        {
            return this.InvokeWebService(url, null, methodName, args);
        }
        #endregion

        #region public object InvokeWebService(string url, string methodname, object arg)
        /// <summary>
        /// ��̬����Web Service�����뵥������
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="methodName">����Web Service�ķ�������</param>
        /// <param name="arg">Ҫ����Web Service�Ĳ���</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string methodName, object arg)
        {
            return this.InvokeWebService(url, null, methodName, new object[] { arg });
        }
        #endregion

        #region public object InvokeWebService(string url, string classname, string methodname, object[] args)
        /// <summary>
        /// ��̬����Web Service
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="className">Ҫ����Web Service���������</param>
        /// <param name="methodName">Ҫ����Web Service�ķ�������</param>
        /// <param name="args">Ҫ����Web Service�Ĳ����б�</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string className, string methodName, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((className == null) || (className == ""))
            {
                className = this.GetWsClassName(url);
            }

            try
            {
                //��ȡWSDL
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(url + "?WSDL");
                ServiceDescription serviceDescription = ServiceDescription.Read(stream);
                ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter();
                serviceDescriptionImporter.AddServiceDescription(serviceDescription, "", "");
                CodeNamespace codeNamespace = new CodeNamespace(@namespace);

                //���ɿͻ��˴��������
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(codeNamespace);
                serviceDescriptionImporter.Import(codeNamespace, codeCompileUnit);
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                ICodeCompiler codeCompiler = codeProvider.CreateCompiler();

                //�趨�������
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");

                //���������
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

                //���ɴ���ʵ���������÷���
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
        /// ͨ��Web Service��URL��ȡ����
        /// </summary>
        /// <param name="webServiceURL">Web Service��URL��ַ</param>
        /// <returns>Web Service������</returns>
        private string GetWsClassName(string webServiceURL)
        {
            string[] parts = webServiceURL.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }

}
