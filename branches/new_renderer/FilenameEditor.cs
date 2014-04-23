using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public class FilenameEditor : System.Drawing.Design.UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if ((context != null) && (context.Instance != null))
			{
				return UITypeEditorEditStyle.Modal;
			}
			return UITypeEditorEditStyle.None;
		}

		[RefreshProperties(RefreshProperties.All)]
		public override Object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			if (context == null || provider == null || context.Instance == null)
			{
				return base.EditValue(provider, value);
			}

			FileDialog fileDlg = null;
			if (context.PropertyDescriptor.Attributes[typeof(SaveFileAttribute)] == null)
			{
				fileDlg = new OpenFileDialog();
			}
			else
			{
				fileDlg = new SaveFileDialog();
			}
			fileDlg.Title = "Select " + context.PropertyDescriptor.DisplayName;
			fileDlg.FileName = value as string;

			FileDialogFilterAttribute filterAtt = context.PropertyDescriptor.Attributes[typeof(FileDialogFilterAttribute)] as FileDialogFilterAttribute;
			if ((filterAtt != null))
			{
				fileDlg.Filter = filterAtt.Filter;
			}
			if (fileDlg.ShowDialog() == DialogResult.OK)
			{
				value = fileDlg.FileName;
			}
			fileDlg.Dispose();
			return value;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class FileDialogFilterAttribute : Attribute
	{

		private string _filter;
		//"Text files (*.txt)|*.txt|All files (*.*)|*.*"
		public string Filter
		{
			get
			{
				return this._filter;
			}
		}

		public FileDialogFilterAttribute(string filter)
			: base()
		{
			this._filter = filter;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class SaveFileAttribute : Attribute
	{

	}
}
