using System.Collections.Generic;

namespace MMORL.Client.Interface
{
    public class UserInterface
    {
        private readonly List<UiElement> _elements = new List<UiElement>();
        public IReadOnlyCollection<UiElement> Elements => _elements.AsReadOnly();

        public void Update(float delta)
        {
            foreach (UiElement element in _elements)
            {
                element.Update(delta);
            }
        }

        public void Add(UiElement element)
        {
            _elements.Add(element);
        }
    }
}
