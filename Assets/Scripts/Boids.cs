using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boids : MonoBehaviour
{
    public class Entity
    {
        public GameObject gameObject;
        public Vector3 velocity;

        public void Update(Boids boids)
        {
            List<Entity> boidsInRange = boids.entities.FindAll(b => b != this && 
                                            (b.gameObject.transform.position - gameObject.transform.position).magnitude <= boids.radius);

            Vector3 acceleration = boids.alignmentAmount * Alignment(boidsInRange);
            acceleration += boids.cohesionAmount * Cohesion(boidsInRange);
            acceleration += boids.separationAmount * Separation(boids, boidsInRange);

            velocity += acceleration;

            // Clamp magnitude
            if (velocity.sqrMagnitude > boids.maxSpeed * boids.maxSpeed)
            {
                velocity = velocity.normalized * boids.maxSpeed;
            }

            gameObject.transform.position += Time.deltaTime * velocity;
            
            float lerp = Mathf.Exp(-boids.rotationSpeed * Time.deltaTime);
            gameObject.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(velocity, Vector3.up), gameObject.transform.rotation, lerp);
        }

        private Vector3 Alignment(List<Entity> boidsInRange)
        {
            Vector3 direction = Vector3.zero;

            foreach (Entity boid in boidsInRange)
            {
                direction += boid.velocity;
            }
            direction /= boidsInRange.Count;

            return direction.normalized;
        }

        private Vector3 Separation(Boids boids, List<Entity> boidsInRange)
        {
            Vector3 direction = Vector3.zero;

            boidsInRange = boidsInRange.FindAll(b => (b.gameObject.transform.position - gameObject.transform.position).magnitude <= boids.radius / 2 &&
                                            Vector3.Dot(b.velocity, velocity) > boids.directionThreshold);
            foreach (Entity boid in boidsInRange)
            {
                Vector3 diff = gameObject.transform.position - boid.gameObject.transform.position;
                direction += diff * (Mathf.Clamp01(1f - diff.magnitude / (boids.radius / 2f)) / diff.magnitude);
            }

            return direction.normalized;
        }

        private Vector3 Cohesion(List<Entity> boidsInRange)
        {
            Vector3 sumPositions = Vector3.zero;

            foreach (Entity boid in boidsInRange)
            {
                sumPositions += boid.gameObject.transform.position;
            }

            Vector3 direction = (sumPositions / boidsInRange.Count) - gameObject.transform.position;
            return direction.normalized;
        }
    }

    [SerializeField] GameObject _entityPrefab;
    [SerializeField] Area _area;
    [SerializeField] int boidsCount = 50;
    [SerializeField] float _maxSpeed = 5f;
    public float maxSpeed => _maxSpeed;
    [SerializeField] float _radius = 2f;
    public float radius => _radius;
    [Range(-1f, 1f)]
    [SerializeField] float _directionThreshold = -0.5f;
    public float directionThreshold => _directionThreshold;
    [SerializeField] float _rotationSpeed = 20f;
    public float rotationSpeed => _rotationSpeed;
    [Range(0f, 10f)]
    [SerializeField] float _alignmentAmount = 1f;
    public float alignmentAmount => _alignmentAmount;
    [Range(0f, 10f)]
    [SerializeField] float _separationAmount = 1f;
    public float separationAmount => _separationAmount;
    [Range(0f, 10f)]
    [SerializeField] float _cohesionAmount = 1f;
    public float cohesionAmount => _cohesionAmount;

    List<Entity> _entities = new List<Entity>();
    public List<Entity> entities => _entities;

    void Start()
    {
        for (int i = 0; i < boidsCount; i++)
        {
            Entity entity = new Entity();
            entity.gameObject = Instantiate(_entityPrefab, gameObject.transform);
            entity.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            entity.gameObject.transform.position = new Vector3(Random.Range(-_area.width / 2, _area.width / 2), Random.Range(-_area.height / 2f, _area.height / 2f), Random.Range(-_area.depth / 2f, _area.depth / 2f));
            _entities.Add(entity);
        }
    }

    void Update()
    {
        foreach (Entity entity in _entities)
        {
            entity.Update(this);

            if (!_area.IsInBound(entity.gameObject.transform.position))
            {
                entity.gameObject.transform.position = _area.GetInBoundPosition(entity.gameObject.transform.position);
            }
        }
    }
}
