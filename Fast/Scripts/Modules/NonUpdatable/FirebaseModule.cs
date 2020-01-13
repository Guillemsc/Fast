using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// To use this, go to Unity Project Settings -> Other settings -> Scripting Define Simbols
// and add the two preprocessor directives when necessary
// USING_FIREBASE
// USING_GOOGLE_PLAY_SERVICES

#if USING_FIREBASE

using Firebase.Auth;

#endif

#if USING_GOOGLE_PLAY_SERVICES

using GooglePlayGames;

#endif

namespace Fast.Modules
{
    public enum FirebaseAuthType
    {
        EMAIL_PASSWORD,
        GOOGLE_PLAY,
    }

    public class FirebaseLoginObj
    {
        private FirebaseAuthType auth_type = new FirebaseAuthType();
        private string firebase_user_id = "";

        public FirebaseLoginObj(FirebaseAuthType auth_type, string firebase_user_id)
        {
            this.auth_type = auth_type;
            this.firebase_user_id = firebase_user_id;
        }

        public string FirebaseUserId
        {
            get { return firebase_user_id; }
        }

        public FirebaseAuthType AuthType
        {
            get { return auth_type; }
        }
    }

    class FirebaseModule : Module
    {
        private FirebaseAuthType auth_to_use = new FirebaseAuthType();

#if USING_FIREBASE && !UNITY_WEBGL

        private Firebase.Auth.FirebaseAuth auth = null;

        private Firebase.Auth.FirebaseUser user = null;

#endif

        public override void Awake()
        {

#if USING_FIREBASE

            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

#endif

        }

        private void ChooseAuthType()
        {
            auth_to_use = FirebaseAuthType.EMAIL_PASSWORD;

#if UNITY_ANDROID && !UNITY_EDITOR

            auth_type = AuthType.GOOGLE_PLAY;

#endif

#if UNITY_IOS && !UNITY_EDITOR

            auth_type = AuthType.GOOGLE;

#endif
        }

        public void LogOut()
        {

#if USING_FIREBASE && !UNITY_WEBGL

            if (user != null)
            {
                auth.SignOut();

                user = null;
            }

#endif

        }

        public void LoginEmailPassword(string email, string password, Action<FirebaseLoginObj> on_success, Action<string> on_fail)
        {

#if USING_FIREBASE && !UNITY_WEBGL

            Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

            LoginWithCredentials(credential,
            delegate (string user_id)
            {
                FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.EMAIL_PASSWORD, user_id);

                if (on_success != null)
                    on_success.Invoke(ret);
            }
            , delegate ()
            {
                if (on_fail != null)
                    on_fail.Invoke("");
            });

#endif

        }

        public void LoginGooglePlay(Action<FirebaseLoginObj> on_success, Action<string> on_google_play_fail,
            Action<string> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE && USING_GOOGLE_PLAY_SERVICES

            UnityEngine.Social.Active.localUser.Authenticate(delegate(bool google_play_success, string google_play_error)
            {
                if (google_play_success)
                {
                    PlayGamesLocalUser user = (PlayGamesLocalUser)Social.Active.localUser;

                    string google_play_auth_code = PlayGamesPlatform.Instance.GetServerAuthCode();

                    Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(google_play_auth_code);

                    LoginWithCredentials(credential,
                    delegate (string user_id)
                    {
                        FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.GOOGLE_PLAY, user_id);

                        if (on_success != null)
                            on_success.Invoke(ret);
                    }
                    , delegate ()
                    {
                        if (on_fail != null)
                            on_fail.Invoke("");
                    });
                }
                else
                {
                    if (on_google_play_fail != null)
                        on_google_play_fail.Invoke(google_play_error);
                }
            });

#endif

        }

        public void LoginIOS(Action<FirebaseLoginObj> on_success, Action<string> on_google_play_fail,
            Action<string> on_fail)
        {

//#if UNITY_ANDROID && USING_FIREBASE && USING_GOOGLE_PLAY_SERVICES

//            UnityEngine.Social.Active.localUser.Authenticate(delegate (bool google_play_success, string google_play_error)
//            {
//                if (google_play_success)
//                {
//                    PlayGamesLocalUser user = (PlayGamesLocalUser)Social.Active.localUser;

//                    string google_play_auth_code = PlayGamesPlatform.Instance.GetServerAuthCode();

//                    Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(google_play_auth_code);

//                    LoginWithCredentials(credential,
//                    delegate (string user_id)
//                    {
//                        FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.GOOGLE_PLAY, user_id);

//                        if (on_success != null)
//                            on_success.Invoke(ret);
//                    }
//                    , delegate ()
//                    {
//                        if (on_fail != null)
//                            on_fail.Invoke("");
//                    });
//                }
//                else
//                {
//                    if (on_google_play_fail != null)
//                        on_google_play_fail.Invoke(google_play_error);
//                }
//            });

//#endif

        }

        private void LoginWithCredentials(Firebase.Auth.Credential credential, Action<string> on_success, Action on_fail)
        {

#if USING_FIREBASE && !UNITY_WEBGL

            if (user != null)
            {
                LogOut();
            }

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                string error_msg = "";
                Exception exception = null;

                bool has_errors = task.HasErrors(out error_msg, out exception);

                if (!has_errors)
                {
                    Firebase.Auth.FirebaseUser curr_user = task.Result;

                    curr_user.TokenAsync(false).ContinueWith(token_task =>
                    {
                        has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            user = curr_user;

                            if (on_success != null)
                                on_success.Invoke(token_task.Result);
                        }
                        else
                        {
                            if (on_fail != null)
                                on_fail.Invoke();
                        }

                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke();
                }
            }
            , TaskScheduler.FromCurrentSynchronizationContext());

#endif

        }

        public void RegisterEmailPassword(string username, string email, string password, string password_conf,
            Action on_success, Action<string> on_fail)
        {

#if USING_FIREBASE && !UNITY_WEBGL

            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                string error_msg = "";
                Exception exception = null;

                bool has_errors = task.HasErrors(out error_msg, out exception);

                if (!has_errors)
                {
                    UpdateDisplayName(username,
                    delegate ()
                    {
                        user = task.Result;

                        if (on_success != null)
                            on_success.Invoke();
                    }
                    , delegate(string err)
                    {
                        if (on_fail != null)
                            on_fail.Invoke(err);
                    });
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

#endif

        }

        public void UpdateDisplayName(string display_name, Action on_success, Action<string> on_fail)
        {

#if USING_FIREBASE && !UNITY_WEBGL

            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = display_name,
            };

            user.UpdateUserProfileAsync(profile).ContinueWith(update_task =>
            {
                string error_msg = "";
                Exception exception = null;

                bool has_errors = update_task.HasErrors(out error_msg, out exception);

                if (!has_errors)
                {
                    if (on_success != null)
                        on_success.Invoke();
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke("");
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

#endif

        }
    }
}

