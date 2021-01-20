git filter-branch --force --index-filter "git rm --cached --ignore-unmatch NinKode.Database/Service/v1/CodeV1Service.cs" --prune-empty --tag-name-filter cat -- --all
