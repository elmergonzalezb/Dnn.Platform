﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

#endregion

namespace DotNetNuke.Web.UI.WebControls.Internal
{
    ///<remarks>
    /// This control is only for internal use, please don't reference it in any other place as it may be removed in future.
    /// </remarks>
    public class DnnFormComboBoxItem : DnnFormListItemBase
    {
        //public DropDownList ComboBox { get; set; }
        public DnnComboBox ComboBox { get; set; }

        private void IndexChanged(object sender, EventArgs e)
        {
            UpdateDataSource(Value, ComboBox.SelectedValue, DataField);
        }

        protected override void BindList()
        {
            BindListInternal(ComboBox, Value, ListSource, ListTextField, ListValueField);
        }

        //internal static void BindListInternal(DropDownList comboBox, object value, IEnumerable listSource, string textField, string valueField)
        internal static void BindListInternal(DnnComboBox comboBox, object value, IEnumerable listSource, string textField, string valueField)
        {
            if (comboBox != null)
            {
                string selectedValue = !comboBox.Page.IsPostBack ? Convert.ToString(value) : comboBox.SelectedValue;

                if (listSource is Dictionary<string, string>)
                {
                    var items = listSource as Dictionary<string, string>;
                    foreach (var item in items)
                    {
                        //comboBox.Items.Add(new ListItem(item.Key, item.Value));
                        comboBox.AddItem(item.Key, item.Value);
                    }
                }
                else
                {
                    comboBox.DataTextField = textField;
                    comboBox.DataValueField = valueField;
                    comboBox.DataSource = listSource;

                    comboBox.DataBind();
                }

                //Reset SelectedValue
                //comboBox.Select(selectedValue);
                var selectedItem = comboBox.FindItemByValue(selectedValue);
                if (selectedItem != null)
                    selectedItem.Selected = true;                
            }
        }

        protected override WebControl CreateControlInternal(Control container)
        {
            //ComboBox = new DropDownList { ID = ID + "_ComboBox" };
            ComboBox = new DnnComboBox { ID = ID + "_ComboBox" };
            ComboBox.SelectedIndexChanged += IndexChanged;
            container.Controls.Add(ComboBox);

            if (ListSource != null)
            {
                BindList();
            }

            return ComboBox;
        }
    }
}
