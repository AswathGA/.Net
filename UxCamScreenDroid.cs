using Com.UXCam;
using Draftsy.dependency;
using Draftsy.Droid.Dependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(UxCamScreenDroid))]
namespace Draftsy.Droid.Dependency
{
    public class UxCamScreenDroid : UxCamScreen
    {
        public void setScreenName(string name)
        {
            UXCam.TagScreenName(name);
        }
    }
}