git checkout -b backport ECSish/master
git cherry-pick -x --strategy=subtree master
git push ECSish backport:master
git checkout master
git branch -d backport
UpdateECSish.cmd