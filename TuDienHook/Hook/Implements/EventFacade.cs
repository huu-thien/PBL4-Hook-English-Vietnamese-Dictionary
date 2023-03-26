using System;
using System.Windows.Forms;

namespace Gma.System.MouseKeyHook.Implementation
{
    internal abstract class EventFacade : IKeyboardMouseEvents
    {
        private MouseListener m_MouseListenerCache;


        public event MouseEventHandler MouseMove
        {
            add { GetMouseListener().MouseMove += value; }
            remove { GetMouseListener().MouseMove -= value; }
        }

        public event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add { GetMouseListener().MouseMoveExt += value; }
            remove { GetMouseListener().MouseMoveExt -= value; }
        }

        public event MouseEventHandler MouseClick
        {
            add { GetMouseListener().MouseClick += value; }
            remove { GetMouseListener().MouseClick -= value; }
        }

        public event MouseEventHandler MouseDown
        {
            add { GetMouseListener().MouseDown += value; }
            remove { GetMouseListener().MouseDown -= value; }
        }

        public event EventHandler<MouseEventExtArgs> MouseDownExt
        {
            add { GetMouseListener().MouseDownExt += value; }
            remove { GetMouseListener().MouseDownExt -= value; }
        }

        public event MouseEventHandler MouseUp
        {
            add { GetMouseListener().MouseUp += value; }
            remove { GetMouseListener().MouseUp -= value; }
        }

        public event EventHandler<MouseEventExtArgs> MouseUpExt
        {
            add { GetMouseListener().MouseUpExt += value; }
            remove { GetMouseListener().MouseUpExt -= value; }
        }

        public event MouseEventHandler MouseDoubleClick
        {
            add { GetMouseListener().MouseDoubleClick += value; }
            remove { GetMouseListener().MouseDoubleClick -= value; }
        }

        public event MouseEventHandler MouseDragStarted
        {
            add { GetMouseListener().MouseDragStarted += value; }
            remove { GetMouseListener().MouseDragStarted -= value; }
        }

        public event EventHandler<MouseEventExtArgs> MouseDragStartedExt
        {
            add { GetMouseListener().MouseDragStartedExt += value; }
            remove { GetMouseListener().MouseDragStartedExt -= value; }
        }

        public event MouseEventHandler MouseDragFinished
        {
            add { GetMouseListener().MouseDragFinished += value; }
            remove { GetMouseListener().MouseDragFinished -= value; }
        }

        public event EventHandler<MouseEventExtArgs> MouseDragFinishedExt
        {
            add { GetMouseListener().MouseDragFinishedExt += value; }
            remove { GetMouseListener().MouseDragFinishedExt -= value; }
        }

        public void Dispose()
        {
            if (m_MouseListenerCache != null) m_MouseListenerCache.Dispose();
        }


        private MouseListener GetMouseListener()
        {
            var target = m_MouseListenerCache;
            if (target != null) return target;
            target = CreateMouseListener();
            m_MouseListenerCache = target;
            return target;
        }

        protected abstract MouseListener CreateMouseListener();
    }
}