namespace GitHooksCsharp.Hooks;

/// <summary>
/// Types of hooks that can be executed.
/// </summary>
public enum HookTypes
{
    // Commit Related Hooks
    PreCommit,                    // Before commit creation
    PrepareCommitMsg,            // Before commit message editor
    CommitMsg,                   // After commit message editor
    PostCommit,                  // After commit creation
    PostRewrite,                 // After commands that rewrite commits

    // Patch Related Hooks
    PreApplyPatch,              // Before applying patches
    ApplyPatchMsg,              // Before applying patches with message
    PostApplyPatch,             // After applying patches

    // Push Related Hooks
    PrePush,                    // Before pushing commits
    PostReceive,                // After remote receives commits
    PreReceive,                 // Before remote receives commits
    ProcReceive,                // For processing received commits
    UpdateServerInfo,           // After repository update on server
    PostUpdate,                 // After repository update

    // Merge Related Hooks
    PreMergeCommit,            // Before merge commits
    PreRebase,                 // Before rebasing
    PostMerge,                 // After merge operation
    
    // Checkout Related Hooks
    PreAutoGc,                 // Before automatic garbage collection
    PostCheckout,              // After checkout operation
    PushToCheckout,            // When pushing to current branch
    
    // Reference Related Hooks
    ReferenceTransaction,       // During reference modifications
    PostIndexChange,           // After index modifications

    // Email Related Hooks
    SendEmailValidate,         // Before sending email

    // Monitoring Hooks
    FsMonitorWatchman,         // File system monitoring

    // Perforce Integration Hooks
    P4PreSubmit,              // Before Perforce submit
    P4ChangeList,             // During Perforce changelist creation
    P4PrepareChangelist,      // Before Perforce changelist preparation
    P4PostChangelist,         // After Perforce changelist

    // Additional Standard Hooks
    PrePack,                  // Before packing repository
    PostUnpack,               // After unpacking repository
    PreFetch,                 // Before fetching
    PostFetch,                // After fetching
    PreDelete,                // Before branch/tag deletion
    PostDelete,               // After branch/tag deletion
    PostBuildSuccess,         // After successful build
    PostBuildFailure         // After failed build
}