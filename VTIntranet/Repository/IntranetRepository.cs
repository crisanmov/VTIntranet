using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VTIntranet.intranetdb;
using System.Data.Entity;

namespace VTIntranet.Repository
{
    public class IntranetRepository : IDisposable
    {
        private  intranetContext _context;

        public IntranetRepository()
        {
            _context = new intranetContext();
        }

        private GenericRepository<tblattachments> attachmentRepository;
        private GenericRepository<tblattachmentstags> attachmentstagsRepository;
        private GenericRepository<tblusersprofiles> usersprofilesRepository;
        private GenericRepository<tblusers> usersRepository;
        private GenericRepository<tbluserspermissions> userspermissionsRepository;
        private GenericRepository<tblpermissions> permissionsRepository;
        private GenericRepository<tblprofiles> profilesRepository;
        private GenericRepository<tbltags> tagsRepository;
        private GenericRepository<tblprofilestags> profilestagsRepository;
        private GenericRepository<tbluserstags> userstagsRepository;

        public GenericRepository<tblattachments> AttachmentRepository
        {
            get
            {
                if (this.attachmentRepository == null)
                {
                    this.attachmentRepository = new GenericRepository<tblattachments>(_context);
                }
                return attachmentRepository;
            }
        }

        public GenericRepository<tblattachmentstags> AttachmentsTagsRepository
        {
            get
            {
                if (this.attachmentstagsRepository == null)
                {
                    this.attachmentstagsRepository = new GenericRepository<tblattachmentstags>(_context);
                }
                return attachmentstagsRepository;
            }
        }

        public GenericRepository<tblusersprofiles> UsersProfilesRepository
        {
            get
            {
                if (this.usersprofilesRepository == null)
                {
                    this.usersprofilesRepository = new GenericRepository<tblusersprofiles>(_context);
                }
                return usersprofilesRepository;
            }
        }

        public GenericRepository<tbltags> TagsRepository
        {
            get
            {
                if (this.tagsRepository == null)
                {
                    this.tagsRepository = new GenericRepository<tbltags>(_context);
                }
                return tagsRepository;
            }
        }

        public GenericRepository<tblprofilestags> ProfilesTagsRepository
        {
            get
            {
                if (this.profilestagsRepository == null)
                {
                    this.profilestagsRepository = new GenericRepository<tblprofilestags>(_context);
                }
                return profilestagsRepository;
            }
        }

        public GenericRepository<tbluserstags> UsersTagsRepository
        {
            get
            {
                if (this.userstagsRepository == null)
                {
                    this.userstagsRepository = new GenericRepository<tbluserstags>(_context);
                }
                return userstagsRepository;
            }
        }

        public GenericRepository<tblusers> UsersRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new GenericRepository<tblusers>(_context);
                }
                return usersRepository;
            }
        }

        public GenericRepository<tbluserspermissions> UsersPermissionsRepository
        {
            get
            {
                if (this.userspermissionsRepository == null)
                {
                    this.userspermissionsRepository = new GenericRepository<tbluserspermissions>(_context);
                }
                return userspermissionsRepository;
            }
        }

        public GenericRepository<tblpermissions> PermissionsRepository
        {
            get
            {
                if (this.permissionsRepository == null)
                {
                    this.permissionsRepository = new GenericRepository<tblpermissions>(_context);
                }
                return permissionsRepository;
            }
        }

        public GenericRepository<tblprofiles> ProfilesRepository
        {
            get
            {
                if (this.profilesRepository == null)
                {
                    this.profilesRepository = new GenericRepository<tblprofiles>(_context);
                }
                return profilesRepository;
            }
        }

        public async Task<List<tbltags>> getTags()
        {
            List<tbltags> result = new List<tbltags>();

            result = await _context.tbltags.ToListAsync();

            return result;
        }

        public async Task<List<tblprofilestags>> getProfileTags()
        {
            List<tblprofilestags> result = new List<tblprofilestags>();

            result = await _context.tblprofilestags.ToListAsync();

            return result;
        }

        public async Task<List<tbluserstags>> getUsersTags()
        {
            List<tbluserstags> result = new List<tbluserstags>();

            result = await _context.tbluserstags.ToListAsync();

            return result;
        }

        public async Task<List<tblattachments>> getAttachments()
        {
            List<tblattachments> result = new List<tblattachments>();

            result = await _context.tblattachments.ToListAsync();

            return result;
        }

        public async Task<List<tblattachmentstags>> getAttachmentsTags()
        {
            List<tblattachmentstags> result = new List<tblattachmentstags>();

            result = await _context.tblattachmentstags.ToListAsync();

            return result;
        }

        public List<tblusers> getUsers()
        {
            List<tblusers> lst = new List<tblusers>();

            lst = _context.tblusers.ToList();

            return lst;
        }

        public void Save()
        {

            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
            GC.SuppressFinalize(this);
        }
        #endregion

        public void Rollback()
        {
            _context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());

        }

    }
}