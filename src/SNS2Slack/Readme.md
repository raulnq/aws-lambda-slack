# SNSToSlack

## AWS SAM

Prepare the local environment and deploy the `template.yml`:
```
nuke SamLocalDeploy --slack-web-hook https://webhook.site/1ca63aab-021b-4007-9940-fe5a39494b0a
```
Note: You can use https://webhook.site/ to receive the requests.

## Alarms

You can use the AWS CLI to list all the Alarms:
```
aws sns list-topics --endpoint-url=http://localhost:4566
```

You can use the AWS CLI to register a new Alarm:
```
aws cloudwatch put-metric-alarm --endpoint-url=http://localhost:4566 --alarm-name my-alarm --alarm-description 'This is my slack message' --comparison-operator GreaterThanThreshold --evaluation-periods 1 --alarm-actions arn:aws:sns:us-east-1:000000000000:SlackTopic
```

You can use the AWS CLI to change the state of the Alarm to the following available values:

- OK
- ALARM
- INSUFFICIENT_DATA

```
aws cloudwatch set-alarm-state --endpoint-url=http://localhost:4566 --alarm-name my-alarm --state-reason "Testing" --state-value ALARM
```