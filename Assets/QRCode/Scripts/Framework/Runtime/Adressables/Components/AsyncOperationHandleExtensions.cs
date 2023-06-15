namespace QRCode.Framework
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public static class AsyncOperationHandleExtensions
    {
        /// <summary>
        ///     Binds the lifetime of the handle to the <see cref="gameObjectHandle" />.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObjectHandleram>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AsyncOperationHandle BindTo(this AsyncOperationHandle self, GameObject gameObjectHandle)
        {
            if (gameObjectHandle == null)
            {
                Addressables.Release(self);
                throw new ArgumentNullException(nameof(gameObjectHandle),
                    $"{nameof(gameObjectHandle)} is null so the handle can't be bound and will be released immediately.");
            }

            if (!gameObjectHandle.TryGetComponent(out ReleaseAddressableInstanceEvent releaseEvent))
                releaseEvent = gameObjectHandle.AddComponent<ReleaseAddressableInstanceEvent>();

            return BindTo(self, releaseEvent);
        }

        /// <summary>
        ///     Binds the lifetime of the handle to the <see cref="gameObjectHandle" />.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObjectHandleram>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AsyncOperationHandle<T> BindTo<T>(this AsyncOperationHandle<T> self, GameObject gameObjectHandle)
        {
            if (gameObjectHandle == null)
            {
                Addressables.Release(self);
                throw new ArgumentNullException(nameof(gameObjectHandle),
                    $"{nameof(gameObjectHandle)} is null so the handle can't be bound and will be released immediately.");
            }

            ((AsyncOperationHandle)self).BindTo(gameObjectHandle);
            return self;
        }

        /// <summary>
        ///     Binds the lifetime of the handle to the <see cref="releaseEvent" />.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="releaseEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AsyncOperationHandle BindTo(this AsyncOperationHandle self, IReleaseEvent releaseEvent)
        {
            if (releaseEvent == null)
            {
                Addressables.Release(self);
                throw new ArgumentNullException(nameof(releaseEvent),
                    $"{nameof(releaseEvent)} is null so the handle can't be bound and will be released immediately.");
            }

            void OnRelease()
            {
                Addressables.Release(self);
                releaseEvent.Dispatched -= OnRelease;
            }

            releaseEvent.Dispatched += OnRelease;
            return self;
        }

        /// <summary>
        ///     Binds the lifetime of the handle to the <see cref="releaseEvent" />.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="releaseEvent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AsyncOperationHandle<T> BindTo<T>(this AsyncOperationHandle<T> self, IReleaseEvent releaseEvent)
        {
            if (releaseEvent == null)
            {
                Addressables.Release(self);
                throw new ArgumentNullException(nameof(releaseEvent),
                    $"{nameof(releaseEvent)} is null so the handle can't be bound and will be released immediately.");
            }

            ((AsyncOperationHandle)self).BindTo(releaseEvent);
            return self;
        }
    }
}