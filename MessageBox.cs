using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;

// NOTE: Unsuccessful in getting this to work. DeBard 8/2/17.

/****************************** Module Header ******************************\
Module Name:  MessageBoxCore.cs
Project:      CSASPNETIntelligentErrorPage
Copyright (c) Microsoft Corporation.
 
The sample code demonstrates how to create a MessageBox in asp.net, usually we
often use JavaScript functions "alert()" or "confirm()" to show simple messages
and make a simple choice with customers, but these dialog boxes is very simple,
we can not add any different and complicate controls, images or styles. As we know,
good web sites always have their own web styles, such as typeface and colors, 
and in this situation, JavaScript dialog boxes looks not very well. So this sample
shows how to make an Asp.net MessageBox.

The MessageBoxCore class stores the MessageBox's core html code, we will replace 
the important parts of MessageBox, such as text, title, icons, script and so on.
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/en-us/openness/resources/licenses.aspx#MPL
All other rights reserved.
 
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace MGM_Transformer
{
    public class MessageBox
    {
        // DeBard added 8/2/17.
        private Page _page = new Page();

        public Page Page {
            get { return _page;}
            set {_page = value;}
        }
        // End DeBard added 8/2/17.
        
        public string MessageText
        {
            get;
            set;
        }
        public string MessageTitle
        {
            get;
            set;
        }
        public MessageBoxIcons MessageIcons
        {
            get;
            set;
        }


        public MessageBoxButtons MessageButtons
        {
            get;
            set;
        }


        public MessageBoxStyle MessageStyles
        {
            get;
            set;
        }


        public List<string> SuccessEvent = new List<string>();


        public List<string> FailedEvent = new List<string>();


        /// <summary> 
        /// Define an Asp.net MessageBox instance. 
        /// </summary> 
        public MessageBox()
        {
            this.MessageIcons = MessageBoxIcons.System;
            this.MessageButtons = MessageBoxButtons.Ok;
            this.MessageStyles = MessageBoxStyle.StyleA;
        }


        /// <summary> 
        /// Define an Asp.net MessageBox instance. 
        /// </summary> 
        /// <param name="text">Message Box text property.</param> 
        public MessageBox(string text)
        {
            this.MessageText = text;
            this.MessageIcons = MessageBoxIcons.System;
            this.MessageButtons = MessageBoxButtons.Ok;
            this.MessageStyles = MessageBoxStyle.StyleA;
        }


        /// <summary> 
        /// Define an Asp.net MessageBox instance. 
        /// </summary> 
        /// <param name="text">MessageBox text property.</param> 
        /// <param name="title">MessageBox title property.</param> 
        public MessageBox(string text, string title)
        {
            this.MessageText = text;
            this.MessageTitle = title;
            this.MessageIcons = MessageBoxIcons.System;
            this.MessageButtons = MessageBoxButtons.Ok;
            this.MessageStyles = MessageBoxStyle.StyleA;
        }


        /// <summary> 
        /// Define an Asp.net MessageBox instance. 
        /// </summary>   
        /// <param name="text">MessageBox text property.</param> 
        /// <param name="title">MessageBox title property.</param> 
        /// <param name="icons">MessageBox icon property.</param> 
        /// <param name="buttons">MessageBox button style.</param> 
        public MessageBox(string text, string title, MessageBoxIcons icons, MessageBoxButtons buttons, MessageBoxStyle styles)
        {
            this.MessageText = text;
            this.MessageTitle = title;
            this.MessageIcons = icons;
            this.MessageButtons = buttons;
            this.MessageStyles = styles;
        }


        public string Show(object sender)
        {
            string iconUrl = string.Empty;
            string buttonStyle = string.Empty;
            string buttonHTML = string.Empty;
            string scriptHTML = string.Empty;
            string coreHTML = string.Empty;
            StringBuilder builder = new StringBuilder();
            switch (this.MessageIcons)
            {
                case MessageBoxIcons.None:
                    iconUrl = string.Empty;
                    break;
                case MessageBoxIcons.Warnning:
                    iconUrl = "";
                    break;
                case MessageBoxIcons.Error:
                    iconUrl = "";
                    break;
                case MessageBoxIcons.Question:
                    iconUrl = "";
                    break;
                case MessageBoxIcons.System:
                    iconUrl = "";
                    break;
            }
            switch (this.MessageStyles)
            {
                case MessageBoxStyle.StyleA:
                    buttonStyle = "button_classA";
                    break;
                case MessageBoxStyle.StyleB:
                    buttonStyle = "button_classB";
                    break;
            }
            switch (this.MessageButtons)
            {
                case MessageBoxButtons.Ok:
                    buttonHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonHTML = String.Format(buttonHTML, "OK", buttonStyle, "Yes();");
                    builder.Append(buttonHTML);
                    break;
                case MessageBoxButtons.OKCancel:
                    string buttonOKHTML = string.Empty;
                    string buttonCancelHTML = string.Empty;
                    buttonOKHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonOKHTML = String.Format(buttonOKHTML, "OK", buttonStyle, "Yes();");
                    builder.Append(buttonOKHTML);
                    builder.Append("   ");
                    buttonCancelHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonCancelHTML = String.Format(buttonCancelHTML, "Cancel", buttonStyle, "No();");
                    builder.Append(buttonCancelHTML);
                    break;
                case MessageBoxButtons.YesOrNo:
                    string buttonYesHTML = string.Empty;
                    string buttonNoHTML = string.Empty;
                    buttonYesHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonYesHTML = String.Format(buttonYesHTML, "Yes", buttonStyle, "Yes();");
                    builder.Append(buttonYesHTML);
                    builder.Append("   ");
                    buttonNoHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonNoHTML = String.Format(buttonNoHTML, "No", buttonStyle, "No();");
                    builder.Append(buttonNoHTML);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    string buttonYesCHTML = string.Empty;
                    string buttonNoCHTML = string.Empty;
                    string buttonCancelCHTML = string.Empty;
                    buttonYesCHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonYesCHTML = String.Format(buttonYesCHTML, "Yes", buttonStyle, "Yes();");
                    builder.Append(buttonYesCHTML);
                    builder.Append("   ");
                    buttonNoCHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonNoCHTML = String.Format(buttonNoCHTML, "No", buttonStyle, "No();");
                    builder.Append(buttonNoCHTML);
                    builder.Append("   ");
                    buttonCancelCHTML = MessageBoxCore.MessageBoxButtonHtml;
                    buttonCancelCHTML = String.Format(buttonCancelCHTML, "Cancel", buttonStyle, "No();");
                    builder.Append(buttonCancelCHTML);
                    break;
            }
            string successName = string.Empty;
            string failedName = "1";
            StringBuilder successBuilder = new StringBuilder();
            StringBuilder failedBuilder = new StringBuilder();
            if (SuccessEvent != null && SuccessEvent.Count != 0)
            {
                int eventCounts = SuccessEvent.Count;
                for (int i = 0; i < eventCounts; i++)
                {
                    successBuilder.Append("PageMethods.");
                    successBuilder.Append(SuccessEvent[i].ToString());
                    successBuilder.Append("(null,null,Success, Failed);");
                }
                successName = successBuilder.ToString();
            }
            if (FailedEvent != null && FailedEvent.Count != 0)
            {
                int eventCounts = FailedEvent.Count;
                for (int i = 0; i < eventCounts; i++)
                {
                    failedBuilder.Append("PageMethods.");
                    failedBuilder.Append(FailedEvent[i].ToString());
                    failedBuilder.Append("(null,null,Success, Failed);");
                }
                failedName = failedBuilder.ToString();
            }
            scriptHTML = MessageBoxCore.MessageBoxScript;
            scriptHTML = String.Format(scriptHTML, successName, failedName);
            coreHTML = MessageBoxCore.MessageBoxHTML;
            coreHTML = String.Format(coreHTML, this.MessageTitle, iconUrl, this.MessageText, builder.ToString());
            (_page as Page).ClientScript.RegisterClientScriptBlock(sender.GetType(), "_arg", scriptHTML, true);
            return coreHTML;
        }


        public enum MessageBoxButtons
        {
            Ok,
            OKCancel,
            YesOrNo,
            YesNoCancel
        };


        public enum MessageBoxIcons
        {
            None,
            Warnning,
            Question,
            System,
            Error
        }


        public enum MessageBoxStyle
        {
            StyleA,
            StyleB
        }
    } 

}