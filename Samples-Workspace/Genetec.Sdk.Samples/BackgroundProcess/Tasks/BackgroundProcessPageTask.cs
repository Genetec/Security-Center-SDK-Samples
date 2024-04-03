// Copyright (C) 2022 by Genetec, Inc. All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.

namespace BackgroundProcess.Tasks
{
    using Genetec.Sdk.Workspace.Tasks;

    public class BackgroundProcessPageTask : CreatePageTask<BackgroundProcessPage>
    {
        public BackgroundProcessPageTask() : base(true)
        {
        }
    }
}
