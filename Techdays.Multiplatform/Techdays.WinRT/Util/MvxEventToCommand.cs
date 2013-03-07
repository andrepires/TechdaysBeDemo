﻿using System.Windows.Input;
using Windows.UI.Interactivity;
using Windows.UI.Xaml;

namespace Techdays.WinRT.Util
{
    public class MvxEventToCommand : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = s as MvxEventToCommand;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }
                }));

        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as MvxEventToCommand, e)));

        /// <summary>
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            "MustToggleIsEnabled",
            typeof(bool),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = s as MvxEventToCommand;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }
                }));

        private object _commandParameterValue;

        private bool? _mustToggleValue;

        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }

            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }

            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This property is here for compatibility
        /// with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="CommandParameter" /> property.
        /// </summary>
        public object CommandParameterValue
        {
            get { return this._commandParameterValue ?? this.CommandParameter; }

            set
            {
                _commandParameterValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get { return (bool)this.GetValue(MustToggleIsEnabledProperty); }

            set { SetValue(MustToggleIsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. This property is here for
        /// compatibility with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
        /// </summary>
        public bool MustToggleIsEnabledValue
        {
            get
            {
                return this._mustToggleValue == null
                           ? this.MustToggleIsEnabled
                           : this._mustToggleValue.Value;
            }

            set
            {
                _mustToggleValue = value;
            }
        }

        /// <summary>
        /// This method is here for compatibility
        /// with the Silverlight version.
        /// </summary>
        /// <returns>The FrameworkElement to which this trigger
        /// is attached.</returns>
        private FrameworkElement GetAssociatedObject()
        {
            return AssociatedObject as FrameworkElement;
        }

        /// <summary>
        /// This method is here for compatibility
        /// with the Silverlight 3 version.
        /// </summary>
        /// <returns>The command that must be executed when
        /// this trigger is invoked.</returns>
        private ICommand GetCommand()
        {
            return Command;
        }

        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this
        /// action should be passed to the bound RelayCommand. If this is true,
        /// the command should accept arguments of the corresponding
        /// type (for example RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }

        /// <summary>
        /// Provides a simple way to invoke this trigger programatically
        /// without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            Invoke(null);
        }

        /// <summary>
        /// Executes the trigger.
        /// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
        /// and leave the CommandParameter and CommandParameterValue empty!</para>
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            var command = GetCommand();
            var commandParameter = CommandParameterValue;

            if (commandParameter == null
                && PassEventArgsToCommand)
            {
                commandParameter = MapCommandParameter(parameter);
            }

            if (command != null
                && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        protected virtual object MapCommandParameter(object parameter)
        {
            return parameter;
        }

        private static void OnCommandChanged(
            MvxEventToCommand element,
            DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            var command = (ICommand)e.NewValue;
        }
    }
}