using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.TouchListeners
{
    public class XamlCommand : Base
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(XamlCommand), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(XamlCommand), null);


        public override IElement OnTouch(IElement rootElement, SKTouchEventArgs e)
        {
            var elem = base.OnTouch(rootElement, e);
            if(elem != null && elem is BindableObject)
            {
                if (e.ActionType == SKTouchAction.Pressed)
                {
                    bool stop = false;
                    while (elem != null && stop == false)
                    {
                        var cmd = GetCommand((BindableObject)elem);
                        var param = GetCommandParameter((BindableObject)elem);
                        if (cmd != null)
                        {
                            if (cmd.CanExecute(param))
                            {
                                cmd.Execute(param);
                                e.Handled = true;
                            }
                            stop = true;
                        }
                        else
                        {
                            elem = elem.Parent;
                        }
                    }
                }
            }
            return elem;
        }

        public static ICommand GetCommand(BindableObject bindable)
        {
            return (ICommand)bindable.GetValue(CommandProperty);
        }

        public static object GetCommandParameter(BindableObject bindable)
        {
            return bindable.GetValue(CommandParameterProperty);
        }
    }
}
