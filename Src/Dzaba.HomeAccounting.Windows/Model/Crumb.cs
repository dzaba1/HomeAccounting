using Dzaba.Utils;
using System;

namespace Dzaba.HomeAccounting.Windows.Model
{
    internal sealed class Crumb
    {
        public Crumb(string name, Type viewType, object parameters)
        {
            Require.NotWhiteSpace(name, nameof(name));
            Require.NotNull(viewType, nameof(viewType));

            Name = name;
            ViewType = viewType;
            Parameters = parameters;
        }

        public string Name { get; }
        public Type ViewType { get; }
        public object Parameters { get; }
    }
}
