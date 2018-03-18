using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    // > idle > active >
    public abstract class State
    {
        // the arrow instance to update
        protected Arrow arrow;

        // init (should be called by subclasses)
        protected State(Arrow arrow)
        {
            this.arrow = arrow;
        }

        // move to the next state
        public abstract State next();

        // update arrow
        public abstract void update();

    } // ChoosingState


    public class Idle : State
    {

        public Idle(Arrow arrow)
            : base(arrow)
        {
            // make arrow invisible
            arrow.sRenderer.enabled = false;

            // stop animation playback
            arrow.animator.enabled = false;

            // reset it's values
            arrow.angle = 0;
            arrow.rotationSpeed = ROTATION_SPEED;
        }

        public override State next()
        {
            if (arrow.player.GetComponent<Player>().getHasAirplane())
            {
                return new Active(arrow);
            }
            else
            {
                return this;
            }
        }

        public override void update()
        { }

    } // Idle


    public class Active : State
    {

        public Active(Arrow arrow)
            : base(arrow)
        {
            // show arrow
            arrow.sRenderer.enabled = true;

            // enable animation
            arrow.animator.enabled = true;
        }

        public override State next()
        {
            Player p = arrow.player.GetComponent<Player>();

            float force = float.Parse(Regex.Split(arrow.GetComponent<SpriteRenderer>().sprite.name, "_")[1]);

            p.throwAirplane(arrow.angle, (float) (1 - force * 0.05));

            return new Idle(arrow);
        }

        public override void update()
        {

            // update current angle
            arrow.angle += arrow.rotationSpeed * Time.deltaTime;

            // validate new angle
            while (arrow.angle < -1 * ANGLE_OFFSET_LIMIT || arrow.angle > ANGLE_OFFSET_LIMIT)
            {
                float diff = Mathf.Abs(ANGLE_OFFSET_LIMIT - Mathf.Abs(arrow.angle));

                if (arrow.angle > ANGLE_OFFSET_LIMIT)
                {
                    arrow.angle -= diff;
                }
                else if (arrow.angle < -1 * ANGLE_OFFSET_LIMIT)
                {
                    arrow.angle += diff;
                }

                // swap rotation direction
                arrow.rotationSpeed = -1 * arrow.rotationSpeed;
            }
        }

    } // Active

    public Countdown countdown;
    public Menu menu;
    public FinishingLine finishingLine;

    // the diff between the player and this object's y position
    public const float Y_DIFF = 0.8f;

    // abs value of rotation speed in angle/sec
    public const int ROTATION_SPEED = 180;

    // max/min values for offset angle
    public const int ANGLE_OFFSET_LIMIT = 45;

    // the keybind to activate the arrow
    public const KeyCode ACTIVATE_KEY_P1 = KeyCode.Space;
    public const KeyCode ACTIVATE_KEY_P2 = KeyCode.KeypadEnter;
    public KeyCode activateKey;

    // the player object
    public GameObject player;

    // sprite renderer
    public SpriteRenderer sRenderer;

    // animator
    public Animator animator;

    // the current state
    public State state;

    // the current angle (relative to player)
    public float angle;

    // the current rotation speed
    public float rotationSpeed;


    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        state = new Idle(this);
        angle = 0;
        rotationSpeed = ROTATION_SPEED;

        if (player.gameObject.name.Equals("Player1")) {
            activateKey = ACTIVATE_KEY_P1;
        }
        else
        {
            activateKey = ACTIVATE_KEY_P2;
        }

        // Start the arrow at the same position as the player.
        updateTransform();
    }


    // updates transform according to player
    void updateTransform()
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        transform.Translate(0, Y_DIFF, 0);
    }


    void Update()
    {
        if (countdown.getGameStart()){
            if (!finishingLine.getGameOver())
            {
                if (!menu.getStopGame())
                {
                    processInput();

                    // no need to update stuff if object isnt visible
                    if (!sRenderer.enabled)
                    {
                        return;
                    }

                    // update transform to match player's
                    updateTransform();

                    // update self
                    state.update();

                    // update transform with new values
                    transform.Rotate(0, 0, angle);
                }
            }
            else
            {
                if (sRenderer.enabled)
                {
                    sRenderer.enabled = false;
                }
            }
        }
    }


    void processInput()
    {
        if (Input.GetKeyDown(activateKey))
        {
            // update current state
            state = state.next();
        }
    }
}