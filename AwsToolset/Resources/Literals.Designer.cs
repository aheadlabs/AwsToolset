//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AwsToolset.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Literals {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Literals() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AwsToolset.Resources.Literals", typeof(Literals).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  The object trying to be uploaded to the bucket already exists. I quit..
        /// </summary>
        public static string b_ObjectTryingToBeUploadedToBucketAlreadyExistsIQuit {
            get {
                return ResourceManager.GetString("b_ObjectTryingToBeUploadedToBucketAlreadyExistsIQuit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Couldn&apos;t get object {0} from bucket {1}. Error: {2}..
        /// </summary>
        public static string p_CouldNotGetObjectFromBucketError {
            get {
                return ResourceManager.GetString("p_CouldNotGetObjectFromBucketError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  An error occurred when listing the objects of the bucket {0}..
        /// </summary>
        public static string p_ErrorListingObjectsOfBucket {
            get {
                return ResourceManager.GetString("p_ErrorListingObjectsOfBucket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Error encountered on server when writing an object to S3. Message: {0}..
        /// </summary>
        public static string p_ErrorWritingObjectToS3 {
            get {
                return ResourceManager.GetString("p_ErrorWritingObjectToS3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Log group {0} NOT reset..
        /// </summary>
        public static string p_LogGroupNotReset {
            get {
                return ResourceManager.GetString("p_LogGroupNotReset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Log group {0} reset..
        /// </summary>
        public static string p_LogGroupReset {
            get {
                return ResourceManager.GetString("p_LogGroupReset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  The object with key {0} was deleted in bucket {1}..
        /// </summary>
        public static string p_ObjectWithKeyWasDeletedInBucket {
            get {
                return ResourceManager.GetString("p_ObjectWithKeyWasDeletedInBucket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  Unknown error encountered on server when writing an object to S3. Message: {0}..
        /// </summary>
        public static string p_UnknownErrorWritingObjectToS3 {
            get {
                return ResourceManager.GetString("p_UnknownErrorWritingObjectToS3", resourceCulture);
            }
        }
    }
}
