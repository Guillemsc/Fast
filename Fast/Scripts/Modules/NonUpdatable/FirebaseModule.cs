#define USING_EASY_MOBILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

// To use this, go to Unity Project Settings -> Other settings -> Scripting Define Simbols
// and add the preprocessor directives when necessary:
// USING_FIREBASE
// USING_GOOGLE_PLAY_SERVICES

namespace Fast.Modules
{
    public enum FirebaseAuthType
    {
        EMAIL_PASSWORD,
        GOOGLE_PLAY_GAMES,
        APPLE_GAME_CENTER,
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

            ChooseAuthType();

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

        // Requieres USING_FIREBASE
        public void LoginEmailPassword(string email, string password, Action<FirebaseLoginObj> on_success, Action<GoogleFirebase.FirebaseErrorType> on_fail)
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
            , on_fail);

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        // Requieres USING_FIREBASE, USING_GOOGLE_PLAY_SERVICES 
        public void LoginGooglePlayGames(Action<FirebaseLoginObj> on_success, Action<string> on_google_play_fail,
            Action<GoogleFirebase.FirebaseErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE && USING_GOOGLE_PLAY_SERVICES

            UnityEngine.Social.Active.localUser.Authenticate(delegate(bool google_play_success, string google_play_error)
            {
                if (google_play_success)
                {
                    GooglePlayGames.PlayGamesLocalUser user = (GooglePlayGames.PlayGamesLocalUser)Social.Active.localUser;

                    string google_play_auth_code = GooglePlayGames.PlayGamesPlatform.Instance.GetServerAuthCode();

                    Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(google_play_auth_code);

                    LoginWithCredentials(credential,
                    delegate (string user_id)
                    {
                        FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.GOOGLE_PLAY_GAMES, user_id);

                        if (on_success != null)
                            on_success.Invoke(ret);
                    }
                    , on_fail);
                }
                else
                {
                    if (on_google_play_fail != null)
                        on_google_play_fail.Invoke(google_play_error);
                }
            });

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        public void LoginGameCenter(Action<FirebaseLoginObj> on_success, Action<string> on_game_center_fail,
            Action<GoogleFirebase.FirebaseErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE

            UnityEngine.Social.Active.localUser.Authenticate(delegate (bool game_center_success, string game_center_error)
            {
                if (game_center_success)
                {
                    Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync().ContinueWith(
                    delegate (Task<Firebase.Auth.Credential> task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            LoginWithCredentials(task.Result,
                            delegate (string user_id)
                            {
                                FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.APPLE_GAME_CENTER, user_id);

                                if (on_success != null)
                                    on_success.Invoke(ret);
                            }
                            , on_fail);
                        }
                        else
                        {
                            Firebase.FirebaseException firebase_exception = exception as Firebase.FirebaseException;
                            GoogleFirebase.FirebaseErrorType error = GoogleFirebase.FirebaseExceptionToFirebaseError.Get(firebase_exception);

                            if (on_fail != null)
                                on_fail.Invoke(error);
                        }
                    });
                }
                else
                {
                    if (game_center_error != null)
                        on_game_center_fail.Invoke(game_center_error);
                }
            });

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        public void LoginMobile(Action<FirebaseLoginObj> on_success, Action<string> on_service_fail,
            Action<GoogleFirebase.FirebaseErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE

            LoginGooglePlayGames(on_success, on_service_fail, on_fail);

#elif UNITY_IOS && USING_FIREBASE

            LoginGameCenter(on_success, on_service_fail, on_fail)

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        // Requieres USING_FIREBASE
        private void LoginWithCredentials(Firebase.Auth.Credential credential, Action<string> on_success, Action<GoogleFirebase.FirebaseErrorType> on_fail)
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
                            Firebase.FirebaseException firebase_exception = exception as Firebase.FirebaseException;
                            GoogleFirebase.FirebaseErrorType error = GoogleFirebase.FirebaseExceptionToFirebaseError.Get(firebase_exception);

                            if (on_fail != null)
                                on_fail.Invoke(error);
                        }

                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    Firebase.FirebaseException firebase_exception = exception as Firebase.FirebaseException;
                    GoogleFirebase.FirebaseErrorType error = GoogleFirebase.FirebaseExceptionToFirebaseError.Get(firebase_exception);

                    if (on_fail != null)
                        on_fail.Invoke(error);
                }
            }
            , TaskScheduler.FromCurrentSynchronizationContext());

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        // Requieres USING_FIREBASE
        public void RegisterEmailPassword(string username, string email, string password, string password_conf,
            Action on_success, Action<GoogleFirebase.FirebaseErrorType> on_fail)
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
                    , on_fail);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }

        // Requieres USING_FIREBASE
        public void UpdateDisplayName(string display_name, Action on_success, Action<GoogleFirebase.FirebaseErrorType> on_fail)
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
                    Firebase.FirebaseException firebase_exception = exception as Firebase.FirebaseException;
                    GoogleFirebase.FirebaseErrorType error = GoogleFirebase.FirebaseExceptionToFirebaseError.Get(firebase_exception);

                    if (on_fail != null)
                        on_fail.Invoke(error);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

#else

            if (on_fail != null)
                on_fail.Invoke(GoogleFirebase.FirebaseErrorType.UNDEFINED);

#endif

        }
    }
}

