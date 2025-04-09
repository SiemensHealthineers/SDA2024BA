## Branching
Default branch of our GIT repository is _**master**_ branch. 

Development is based on **Trunk based branching** in GIT so every feature, bugfix or other code change is merged into **master** branch so merging will not get too complex and messy.
- Pull latest changes on master branch
- Create custom branch from master branch
- Commit changes
- Create Pull request to merge changes into master branch
- Use *squash* commit strategy to keep the commit history clean on master branch
- Delete the branch after completing the Pull request

### Branch naming
- Branch names are separated by dash -
- Branch name consist of [branch type prefix]/[story + work item if scope is work item]-descriptive-name-in-kebab-case
- Branch type prefixes
  - `feature`
  - `bugfix`
  - `refactor`
- Examples
  - `feature/story-125-perfect-code-example`
  - `feature/story-842-task-1182-button-toggle-event`
  - `bugfix/bug-252-logout-keeps-first-page-data`
- Team conventions
  - `branch prefixes shold be: "feature"/"bugfix"/"refactor"`
  - `we don't differentiate between backend and frontend branches`

### Commits
- Commit your changes as soon as possible with some finished part of functionality.
- For description of commits, use [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification