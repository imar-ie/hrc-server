<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ReadBlob" generation="1" functional="0" release="0" Id="c9d9c069-8e4e-4f15-b5b1-8c116b42e949" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ReadBlobGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WorkerRole1:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ReadBlob/ReadBlobGroup/LB:WorkerRole1:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WorkerRole1:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ReadBlob/ReadBlobGroup/MapWorkerRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WorkerRole1:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWorkerRole1:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapWorkerRole1:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapWorkerRole1:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WorkerRole1" generation="1" functional="0" release="0" software="C:\Users\t00164080\Downloads\IMaR\projects\ReadBlob\ReadBlob\bin\Release\ReadBlob.csx\roles\WorkerRole1" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRole1&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1Instances" />
            <sCSPolicyFaultDomainMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="WorkerRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WorkerRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="690189f3-7643-4d83-ac61-a629bac251da" ref="Microsoft.RedDog.Contract\ServiceContract\ReadBlobContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="7b2f920c-733f-42b8-bca1-6148a2cf0fe9" ref="Microsoft.RedDog.Contract\Interface\WorkerRole1:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ReadBlob/ReadBlobGroup/WorkerRole1:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>