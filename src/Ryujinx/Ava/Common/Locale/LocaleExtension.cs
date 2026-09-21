using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Ryujinx.Ava.Common.Locale;
using System;

namespace Ryujinx.Ava.Common.Locale
{
    internal class LocaleExtension : MarkupExtension
    {
        private LocaleKeys? _key;
        private string _keyString;

        public LocaleExtension(LocaleKeys key)
        {
            _key = key;
        }

        public LocaleExtension(string keyString)
        {
            _keyString = keyString;
        }

        public LocaleKeys Key 
        { 
            get 
            {
                if (_key.HasValue)
                {
                    return _key.Value;
                }

                if (Enum.TryParse<LocaleKeys>(_keyString, out var parsedKey))
                {
                    _key = parsedKey;
                    return parsedKey;
                }

                throw new ArgumentException($"Unable to convert System.String to Ryujinx.Ava.Common.Locale.LocaleKeys: '{_keyString}' is not a valid LocaleKeys value.");
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            LocaleKeys keyToUse = Key;

            var binding = new Binding($"[{keyToUse}]")
            {
                Source = LocaleManager.Instance,
            };

            return binding;
        }
    }
}
