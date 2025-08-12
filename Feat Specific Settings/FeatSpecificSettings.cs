using Il2Cpp;
using MelonLoader;
using ModSettings;

namespace FeatSettings
{
    public abstract class FeatSpecificSettingsBase : JsonModSettings
    {
        protected FeatSettingsManager mManager;
        private static bool isFirstInitialization = true;
        private static readonly object initLock = new object();

        public FeatSpecificSettingsBase(FeatSettingsManager manager)
        {
            mManager = manager;
            Initialize();
        }

        protected virtual void Initialize()
        {
            try
            {
                lock (initLock)
                {
                    if (isFirstInitialization)
                    {
                        AddToModSettings("FeatSettings");
                        isFirstInitialization = false;
                    }
                }
                RefreshGUI();
                ApplyAdjustedFeatSettings();
            }
            catch (Exception e)
            {
                mManager.Log($"ERROR in FeatSpecificSettingsBase.Initialize: {e}");
            }
        }

        protected override void OnConfirm()
        {
            try
            {
                ApplyAdjustedFeatSettings();
                FeatSettingsManager.Instance.ApplyAllFeatSettings();
            }
            catch (Exception e)
            {
                mManager.Log($"ERROR in FeatSpecificSettingsBase.onConfirm: {e}");
            }
        }

        public abstract void ApplyAdjustedFeatSettings();
    }

    public abstract class FeatSpecificSettings<T> : FeatSpecificSettingsBase where T : Feat
    {
        protected T? mFeat;

        public T? Feat { get { return mFeat; } }

        public FeatSpecificSettings(FeatSettingsManager manager) : base(manager) { }

        public virtual void Initialize(T tFeat)
        {
            mFeat = tFeat;
        }
    }
}
