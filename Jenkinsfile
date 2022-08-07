pipeline {
  agent any
  stages {
    stage('Restore packages') {
      steps {
        sh 'dotnet restore'
      }
    }

    stage('Publish') {
      steps {
        sh 'dotnet publish'
      }
    }

    stage('Deploy Using SSH Steps STAGING') {
      when {
        branch 'staging' 
      }
      steps {
        script {
          def remote = [:]
          remote.name = "stg.api.eccediciones.com"
          remote.host = "10.13.160.6"
          remote.allowAnyHosts = true

          def backupDir = "/web/backup/ECC_PublishedAPI/${currentDateTime}"
          def localPublishDir = "CoreAPI/bin/Debug/net5.0/publish"
          def localTemplatesDir = "CoreAPI/Templates"
          def remotePublishDir = "/web/html"

          withCredentials([sshUserPrivateKey(credentialsId: 'ECC-AD-OV2-APIXX-Devel', keyFileVariable: 'JENKINS_RSA', usernameVariable: 'USER')]) {
            remote.user = USER
            remote.identityFile = JENKINS_RSA

            def backupStructure = "mkdir -p ${backupDir}"
            def backupCurrentRelease = "cp -rp ${remotePublishDir}/ECC_PublishedAPI ${backupDir}"
            def setDirName = "mv ${remotePublishDir}/publish ${remotePublishDir}/ECC_PublishedAPI && chmod -R g+w ${remotePublishDir}/ECC_PublishedAPI"
            def restartApiService = "systemctl restart WebApi"
            sshCommand remote: remote, command: backupStructure, failOnError: 'true'
            sshCommand remote: remote, command: backupCurrentRelease, failOnError: 'true'
            sshRemove remote: remote, path: "${remotePublishDir}/ECC_PublishedAPI"
            sshPut remote: remote, from: localPublishDir, into: remotePublishDir
            sshCommand remote: remote, command: setDirName , failOnError: 'true'
            sshPut remote: remote, from: localTemplatesDir, into: "${remotePublishDir}/ECC_PublishedAPI"
            sshRemove remote: remote, path: "${remotePublishDir}/ECC_PublishedAPI/appsettings.Development.json"
            sshCommand remote: remote, command: restartApiService, failOnError: 'true', sudo: true
          }
        }

      }
    }
    stage('SSH Deploy on Frontend Servers') {
       when {
        branch 'master' 
      }
      steps {
        script {
          remoteservers.split(",").each{ server ->
	          stage ("Server $server"){
              def remote = [:]
              remote.name = "api.eccediciones.com"
              remote.host = "$server"
              remote.allowAnyHosts = true
    
              def backupDir = "/web/backup/ECC_PublishedAPI/${currentDateTime}"
              def localPublishDir = "CoreAPI/bin/Debug/net5.0/publish"
              def localTemplatesDir = "CoreAPI/Templates"
              def remotePublishDir = "/web/html"
    
              withCredentials([sshUserPrivateKey(credentialsId: 'ECC-AD-OV2-APIXX-Devel', keyFileVariable: 'JENKINS_RSA', usernameVariable: 'USER')]) {
                remote.user = USER
                remote.identityFile = JENKINS_RSA
    
                def backupStructure = "mkdir -p ${backupDir}"
                def backupCurrentRelease = "cp -rp ${remotePublishDir}/ECC_PublishedAPI ${backupDir}"
                def setDirName = "mv ${remotePublishDir}/publish ${remotePublishDir}/ECC_PublishedAPI && chmod -R g+w ${remotePublishDir}/ECC_PublishedAPI"
                def restartApiService = "systemctl restart WebApi"
                sshCommand remote: remote, command: backupStructure, failOnError: 'true'
                sshCommand remote: remote, command: backupCurrentRelease, failOnError: 'true'
                sshRemove remote: remote, path: "${remotePublishDir}/ECC_PublishedAPI"
                sshPut remote: remote, from: localPublishDir, into: remotePublishDir
                sshCommand remote: remote, command: setDirName , failOnError: 'true'
                sshPut remote: remote, from: localTemplatesDir, into: "${remotePublishDir}/ECC_PublishedAPI"
                sshRemove remote: remote, path: "${remotePublishDir}/ECC_PublishedAPI/appsettings.Development.json"
                sshCommand remote: remote, command: restartApiService, failOnError: 'true', sudo: true
              }
            }
	        }
        }
      }
    }
  }
  post {
    success {
      slackSend color: "good", message: "CoreApi deployed successfully - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
      script {
        if (env.BRANCH_NAME == 'staging'){
          def remote = [:]
          remote.name = "stg.api.eccediciones.com"
          remote.host = "10.13.160.6"
          remote.allowAnyHosts = true

          withCredentials([sshUserPrivateKey(credentialsId: 'ECC-AD-OV2-APIXX-Devel', keyFileVariable: 'JENKINS_RSA', usernameVariable: 'USER')]) {
            remote.user = USER
            remote.identityFile = JENKINS_RSA

            def rmBackupHistory = 'for i in $(ls -rtd /web/backup/ECC_PublishedAPI/* | grep -v "$(ls -rtd /web/backup/ECC_PublishedAPI/*  | tail -2)"); do echo "Borrando $i ..."; rm -rf $i; done '

            sshCommand remote: remote, command: rmBackupHistory, failOnError: 'true'
          }
        }
      }
      script {
        if (env.BRANCH_NAME == 'master'){
          remoteservers.split(",").each{ server ->
            def remote = [:]
            remote.name = "api.eccediciones.com"
            remote.host = "$server"
            remote.allowAnyHosts = true

            withCredentials([sshUserPrivateKey(credentialsId: 'ECC-AD-OV2-APIXX-Devel', keyFileVariable: 'JENKINS_RSA', usernameVariable: 'USER')]) {
              remote.user = USER
              remote.identityFile = JENKINS_RSA

              def rmBackupHistory = 'for i in $(ls -rtd /web/backup/ECC_PublishedAPI/* | grep -v "$(ls -rtd /web/backup/ECC_PublishedAPI/*  | tail -2)"); do echo "Borrando $i ..."; rm -rf $i; done '

              sshCommand remote: remote, command: rmBackupHistory, failOnError: 'true'
            }
          }
        }
      }
    }
    failure {
      slackSend color: "danger", failOnError:true, message:"CoreApi build failed  - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
    }
  }
  environment {
    currentDateTime = sh(returnStdout: true, script: 'date +%d%m%Y%H%M').trim()
    remoteServers = "10.13.160.53,10.13.160.3"
  }
  triggers {
    githubPush()
  }
}
