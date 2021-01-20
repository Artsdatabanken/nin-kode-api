echo "Posting to slack"
curl -X POST -H 'Content-type: application/json' --data '{"text":"deploy nin-kode-api-test"}' $slackaddy