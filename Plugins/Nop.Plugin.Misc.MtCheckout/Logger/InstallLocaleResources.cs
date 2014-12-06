namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger
{
    using ;
    using Nop.Core;
    using Nop.Core.Data;
    using Nop.Core.Domain.Localization;
    using Nop.Core.Infrastructure;
    using Nop.Services.Localization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;

    public class InstallLocaleResources
    {
        private readonly FNSLogger ;
        private readonly bool ;
        private readonly string ;

        private void (bool  = true)
        {
            ILanguageService service = EngineContext.Current.Resolve<ILanguageService>();
            EngineContext.Current.Resolve<IRepository<Language>>();
            IWebHelper helper = EngineContext.Current.Resolve<IWebHelper>();
            ILocalizationService service2 = EngineContext.Current.Resolve<ILocalizationService>();
            if (helper == null)
            {
                throw new ArgumentNullException(..(0x2241));
            }
            if (service2 == null)
            {
                throw new ArgumentNullException(..(0x220f));
            }
            if (service == null)
            {
                throw new ArgumentNullException(..(0x222c));
            }
            foreach (string str in Directory.EnumerateFiles(helper.MapPath(this.), ..(0x224e), SearchOption.TopDirectoryOnly))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(..(0x2257) + str);
                string str2 = ..(0x9b);
                string str3 = ..(0x9b);
                using (XmlTextReader reader = new XmlTextReader(str))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == ..(0x2290))
                                {
                                    string attribute = reader.GetAttribute(..(0x22a5));
                                    if (!string.IsNullOrWhiteSpace(attribute))
                                    {
                                        str2 = attribute.Trim();
                                    }
                                }
                                break;

                            case XmlNodeType.Text:
                                if (!string.IsNullOrWhiteSpace(reader.Value))
                                {
                                    str3 = reader.Value.Trim();
                                    this.(service2, service, str2, str3, );
                                    builder.AppendLine(string.Format(..(0x22ae), str2, str3));
                                }
                                str2 = ..(0x9b);
                                str3 = ..(0x9b);
                                break;

                            case XmlNodeType.EndElement:
                                str2 = ..(0x9b);
                                str3 = ..(0x9b);
                                break;
                        }
                    }
                }
                this.LogMessage(builder.ToString());
            }
        }

        private void (ILocalizationService , ILanguageService , string , string , bool )
        {
            if ( == null)
            {
                throw new ArgumentNullException(..(0x220f));
            }
            if ( == null)
            {
                throw new ArgumentNullException(..(0x222c));
            }
            if (!string.IsNullOrWhiteSpace() && !string.IsNullOrWhiteSpace())
            {
                foreach (Language language in .GetAllLanguages(true, 0))
                {
                    LocaleStringResource localeStringResource = .GetLocaleStringResourceByName(, language.Id, false);
                    if (localeStringResource == null)
                    {
                        localeStringResource = new LocaleStringResource {
                            LanguageId = language.Id,
                            ResourceName = ,
                            ResourceValue = 
                        };
                        .InsertLocaleStringResource(localeStringResource);
                    }
                    else if ()
                    {
                        localeStringResource.ResourceValue = ;
                        .UpdateLocaleStringResource(localeStringResource);
                    }
                }
            }
        }

        public InstallLocaleResources(string filepath, bool showDebugInfo = false)
        {
            this. = filepath;
            this. = showDebugInfo;
            this. = new FNSLogger(showDebugInfo);
        }

        public void Install()
        {
            this.(true);
        }

        public void LogMessage(string message)
        {
            if (this.)
            {
                this..LogMessage(message);
            }
        }

        public void UnInstall(string languageResourceMask)
        {
            InstallLocaleResources.  = new InstallLocaleResources. {
                 = languageResourceMask
            };
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x22e3) + .);
            if (!string.IsNullOrWhiteSpace(.))
            {
                IRepository<LocaleStringResource> repository = EngineContext.Current.Resolve<IRepository<LocaleStringResource>>();
                ParameterExpression expression = Expression.Parameter(typeof(LocaleStringResource), ..(0x2330));
                foreach (LocaleStringResource resource in repository.Table.Where<LocaleStringResource>(Expression.Lambda<Func<LocaleStringResource, bool>>(Expression.Call(Expression.Property(expression, (MethodInfo) methodof(LocaleStringResource.get_ResourceName)), (MethodInfo) methodof(string.Contains), new Expression[] { Expression.Constant(.) }), new ParameterExpression[] { expression })).ToList<LocaleStringResource>())
                {
                    builder.AppendLine(..(0x2335) + resource.ResourceName + ..(0x2376) + resource.ResourceValue);
                    repository.Delete(resource);
                }
                builder.AppendLine(..(0x238f));
                this.LogMessage(builder.ToString());
            }
        }

        public void Update()
        {
            this.(false);
        }

        private class  : LocaleStringResource
        {
            [CompilerGenerated]
            private bool ;
            public IList<FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger.InstallLocaleResources.>  = new List<FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger.InstallLocaleResources.>();
            [CompilerGenerated]
            private string ;

            public (XmlNode localStringResource, string nameSpace = "")
            {
                this.(nameSpace);
                XmlAttribute attribute = localStringResource.Attributes[..(0x22a5)];
                XmlNode node = localStringResource.SelectSingleNode(..(0x23c4));
                if (attribute == null)
                {
                    throw new NopException(..(0x23cd));
                }
                string str = attribute.Value.Trim();
                if (string.IsNullOrEmpty(str))
                {
                    throw new NopException(..(0x241e));
                }
                base.ResourceName = str;
                if ((node == null) || string.IsNullOrEmpty(node.InnerText.Trim()))
                {
                    this.(false);
                }
                else
                {
                    this.(true);
                    base.ResourceValue = node.InnerText.Trim();
                }
                foreach (XmlNode node2 in localStringResource.SelectNodes(..(0x246f)))
                {
                    this..Add(new FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger.InstallLocaleResources.(node2, this.()));
                }
            }

            public bool IsPersistable
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public string Namespace
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public string NameWithNamespace
            {
                get
                {
                    string str = this.();
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + ..(0x2490);
                    }
                    return (str + base.ResourceName);
                }
            }
        }

        private class <> : IComparer<>, IComparer
        {
            private readonly Comparison<> ;

            public int ( ,  )
            {
                return this.(, );
            }

            public int (object , object )
            {
                return this.(() , () );
            }

            public (Comparison<> comparison)
            {
                this. = comparison;
            }
        }

        [CompilerGenerated]
        private sealed class 
        {
            public string ;
        }
    }
}

