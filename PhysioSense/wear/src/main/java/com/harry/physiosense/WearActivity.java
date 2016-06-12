package com.harry.physiosense;

//default stuff

import android.app.Activity;
import android.os.Bundle;
import android.support.wearable.view.WatchViewStub;
import android.view.View;
import android.widget.TextView;

// initiating sensors
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;


// initiating debugging
import android.util.Log;
import android.widget.Toast;


// initiating google api client and android wearable client
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.common.api.ResultCallback;
import com.google.android.gms.wearable.MessageApi;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.NodeApi;
import com.google.android.gms.wearable.Wearable;

public class WearActivity extends Activity implements SensorEventListener, GoogleApiClient.ConnectionCallbacks, GoogleApiClient.OnConnectionFailedListener {

    // initiating TAG as string for debugging
    private static final String TAG = "MainActivity";
    public static String SERVICE_CALLED_WEAR = "HR Updated";
    public TextView mTextView;
    Node mNode; // the connected device to send the message to
    GoogleApiClient mGoogleApiClient;
    // Initiating Sensors
    SensorManager mSensorManager;
    //public TextView mTextViewHeart;
    Sensor mHeartRateSensor;
    private boolean mResolvingError = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_wear);

        // enable ambient mode
        //setAmbientEnabled();

        //Connect the GoogleApiClient
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .addApi(Wearable.API)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .build();

        final WatchViewStub stub = (WatchViewStub) findViewById(R.id.watch_view_stub);
        stub.setOnLayoutInflatedListener(new WatchViewStub.OnLayoutInflatedListener() {


            @Override
            public void onLayoutInflated(WatchViewStub stub) {
                //initiating TextView from the layout to display heart rate
                mTextView = (TextView) stub.findViewById(R.id.textHR);

                // mTextViewHeart = (TextView) stub.findViewById(R.id.heart);

                // invoke getHR method
                getHR();


            }
        });
    }

    public void getHR() {
        // initiating sensor manager & service
        mSensorManager = ((SensorManager) getSystemService(SENSOR_SERVICE));
        // initiating heart rate sensor
        mHeartRateSensor = mSensorManager.getDefaultSensor(Sensor.TYPE_HEART_RATE);
        //initiating sensor delay
        mSensorManager.registerListener(this, mHeartRateSensor, SensorManager.SENSOR_DELAY_NORMAL);

        if (mHeartRateSensor == null) {
            // print debugs if the pushing null value (not working)
            Log.d(TAG, "heart rate sensor is null");

        }

    }

    @Override
    public void onSensorChanged(SensorEvent event) {
        if (event.sensor.getType() == Sensor.TYPE_HEART_RATE) {
            String hRate = "" + (int) event.values[0];
            mTextView.setText("Current HR is: " + hRate);
            sendMessage(hRate);

            // debug to see event IDs
            // Log.d(TAG, "Event: " +  event.toString());

            // print the HR value in debug
            Log.d(TAG, "Heart Rate: " + hRate);

        } else
            // print debug for unknown or unavailable sensor
            Log.d(TAG, "Unknown sensor type");
    }

    @Override
    public void onAccuracyChanged(Sensor sensor, int accuracy) {
        // print the accuracy in debug
        // Log.d(TAG, "Accuracy changed by: " + accuracy);
    }

    @Override
    protected void onStart() {
        super.onStart();
        if (!mResolvingError) {
            mGoogleApiClient.connect();
            Log.d(TAG, "Connecting to GoogleApiClient: " + mGoogleApiClient.isConnecting());
            Toast.makeText(getApplicationContext(), "Connecting...", Toast.LENGTH_SHORT).show();
        }
    }

     // Resolve the node = the connected device to send the message to

    private void resolveNode() {

        Wearable.NodeApi.getConnectedNodes(mGoogleApiClient)
                .setResultCallback(new ResultCallback<NodeApi.GetConnectedNodesResult>() {
                    @Override
                    public void onResult(NodeApi.GetConnectedNodesResult nodes) {
                        for (Node node : nodes.getNodes()) {
                            mNode = node;
                        }
                    }
                });
    }

    @Override
    public void onConnected(Bundle bundle) {

        resolveNode();
        Toast.makeText(getApplicationContext(), "Connected", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onConnectionSuspended(int i) {
        Log.d(TAG, "Suspended! - Connected to GoogleApiClient: " + mGoogleApiClient.isConnected());
        Toast.makeText(getApplicationContext(), "Connection Suspended!", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onConnectionFailed(ConnectionResult connectionResult) {
        Log.d(TAG, "Failed - Connected to GoogleApiClient: " + mGoogleApiClient.isConnected());
        Toast.makeText(getApplicationContext(), "Connection Failed!", Toast.LENGTH_SHORT).show();
    }


    // Send message to mobile handheld

    private void sendMessage(String Key) {

        if (mNode != null && mGoogleApiClient != null && mGoogleApiClient.isConnected()) {
            Log.d(TAG, "Connection live: " + mGoogleApiClient.isConnected());
            Wearable.MessageApi.sendMessage(
                    mGoogleApiClient, mNode.getId(), SERVICE_CALLED_WEAR + "--" + Key, null).setResultCallback(

                    new ResultCallback<MessageApi.SendMessageResult>() {
                        @Override
                        public void onResult(MessageApi.SendMessageResult sendMessageResult) {

                            if (!sendMessageResult.getStatus().isSuccess()) {
                                Log.e(TAG, "Failed to send message with status code: "
                                        + sendMessageResult.getStatus().getStatusCode());
                            }
                        }
                    }
            );
        }

    }

    @Override
    protected void onResume() {
        // in case of pausing the sensor, resume the listening without warming up again
        super.onResume();
        mSensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
        mSensorManager.registerListener(this, mSensorManager.getDefaultSensor(Sensor.TYPE_HEART_RATE), SensorManager.SENSOR_DELAY_NORMAL);
        mSensorManager.unregisterListener(this);
    }

    @Override
    protected void onStop() {
        // making sure that the sensor is turned off (unregistered) on exit/app switch
       // mSensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
        //mSensorManager.unregisterListener(this);
    super.onStop();

    }

    public void StopHR(View view) {
        mSensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
        mSensorManager.unregisterListener(this);
        Log.d(TAG, "STOPPED by the USER");
        this.finish();
        System.exit(0);
    }

}

