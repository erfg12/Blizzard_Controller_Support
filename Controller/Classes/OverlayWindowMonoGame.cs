namespace Blizzard_Controller
{
    // Minimal build-time stub for the legacy MonoGame overlay.
    // The full implementation (MonoGame Game-derived class) was removed during cleanup
    // and some entry points still instantiate this type. Add a small shim so the
    // projects compile. This keeps runtime behavior unchanged until a proper
    // MonoGame-backed overlay is restored.
    public class OverlayWindowMonoGame
    {
        // Synchronous Run() matches existing call sites (Program.Main and App.axaml.cs).
        // Keep this lightweight to avoid pulling in MonoGame assemblies during a plain build.
        public void Run()
        {
            // Intentionally left minimal. At runtime the real overlay may be required
            // to render; for now this stub prevents compile-time errors.
        }
    }
}
