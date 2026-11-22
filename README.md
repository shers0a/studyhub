# StudyHub

## RULES:
- The 'master' branch always runs clean, so do not work on there directly
- Each single feature must be worked on independently on a separate branch (ex: feature/<feature_name>)
- Before every merge: 'git pull origin master' in your branch, verify that 'dotnet build' runs without errors and then open a PR on GitHub
- Migrations are NOT done simultaneously: just a single PR opened at one time
- Commit messages describe what has been done